using EncuestaApp.Models;
using EncuestasApp.Models;
using SQLite;

namespace EncuestaApp.Services;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService(string dbPath)
    {

        _database = new SQLiteAsyncConnection(dbPath);
  
        _database.CreateTableAsync<Encuesta>().Wait();
        _database.CreateTableAsync<Usuario>().Wait();
        _database.CreateTableAsync<CodigoPostalCat>();
        _database.CreateTableAsync<Localidad>();
    

    }

    public static class DeviceService
    {
        public static string GetDeviceId()
        {
#if ANDROID
            var context = Android.App.Application.Context;
            string? androidId = global::Android.Provider.Settings.Secure.GetString(
                context.ContentResolver,
                global::Android.Provider.Settings.Secure.AndroidId
            );

            // Persistir para mantener siempre el mismo valor en caso de que cambie
            if (string.IsNullOrEmpty(androidId))
            {
                androidId = Preferences.Get("DeviceId", string.Empty);
                if (string.IsNullOrEmpty(androidId))
                {
                    androidId = Guid.NewGuid().ToString();
                    Preferences.Set("DeviceId", androidId);
                }
            }

            return androidId;
#else
        // En iOS/Windows usa un GUID persistente
        var deviceId = Preferences.Get("DeviceId", string.Empty);
        if (string.IsNullOrEmpty(deviceId))
        {
            deviceId = Guid.NewGuid().ToString();
            Preferences.Set("DeviceId", deviceId);
        }
        return deviceId;
#endif
        }
    }

    public static class BatchSettingsService
    {
        private const string BatchSizeKey = "BatchSize";
        private const int DefaultBatchSize = 5;

        public static int GetBatchSize()
        {
            return Preferences.Get(BatchSizeKey, DefaultBatchSize);
        }

        public static void SetBatchSize(int size)
        {
            Preferences.Set(BatchSizeKey, size);
        }
    }


    // Encuestas
    public Task<int> SaveEncuestaAsync(Encuesta encuesta)
    {
        return _database.InsertAsync(encuesta);
    }

    public async Task InicializarCodigosPostalesAsync()
    {
        var total = await _database.Table<CodigoPostalCat>().CountAsync();
        if (total > 0)
            return;

        using var stream = await FileSystem.OpenAppPackageFileAsync("codigos_postales.csv");
        using var reader = new StreamReader(stream);

        bool header = true;

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();

            if (header)
            {
                header = false;
                continue;
            }

            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split(',');

            if (parts.Length < 5)
                continue;

            var registro = new CodigoPostalCat
            {
                Codigo = parts[0].Trim(),
                Colonia = parts[1].Trim(),
                Municipio = parts[2].Trim(),
                cve_mun = parts[3].Trim().PadLeft(3, '0'),
                SeccionElectoral = parts[4].Trim().PadLeft(4,'0')
            };


            await _database.InsertAsync(registro);
        }
    }

    public Task<List<Encuesta>> GetEncuestasAsync()
    {
        return _database.Table<Encuesta>().ToListAsync();
    }

    // Usuarios
    public Task<int> SaveUsuarioAsync(Usuario usuario) =>
        _database.InsertAsync(usuario);

    public Task<Usuario?> GetUsuarioAsync(string username, string password) =>
        _database.Table<Usuario>()
            .Where(u => u.Username == username && u.Password == password)
            .FirstOrDefaultAsync();

    // Sincronizar encuestas
    public Task<List<Encuesta>> GetEncuestasPendientesAsync() =>
        _database.Table<Encuesta>().Where(e => !e.Sincronizada).ToListAsync();

    public Task<int> UpdateEncuestaAsync(Encuesta encuesta) =>
        _database.UpdateAsync(encuesta);

    public async Task DeleteEncuestaAsync(int id)
    {
        var encuesta = await _database.Table<Encuesta>()
                                .Where(e => e.Id == id)
                                .FirstOrDefaultAsync();

        if (encuesta != null)
        {
            await _database.DeleteAsync(encuesta);
        }
    }

    public Task<Encuesta> GetCurpExistenteAsync(string curp)
    {
        return _database.Table<Encuesta>().Where(x => x.Curp == curp).FirstOrDefaultAsync();
    }

    public async Task<List<CodigoPostalCat>> GetColoniasByCPAsync(string codigoPostal)
    {
        if (!codigoPostal.StartsWith("78") && !codigoPostal.StartsWith("79"))
        {
            return new List<CodigoPostalCat>
        {
            new CodigoPostalCat
            {
                Codigo = codigoPostal,
                Colonia = "FUERA DEL ESTADO",
                Municipio = "FUERA DEL ESTADO",
                cve_mun = "9999",
                SeccionElectoral = "9999"
            }
        };
        }

        return await _database
            .Table<CodigoPostalCat>()
            .Where(x => x.Codigo == codigoPostal)
            .ToListAsync();
    }



    public async Task CargarLocalidadesDesdeCsvAsync()
    {
        // Evita recargar si ya hay datos
        var count = await _database.Table<Localidad>().CountAsync();
        if (count > 0)
            return;

        using var stream = await FileSystem.OpenAppPackageFileAsync("localidades.csv");
        using var reader = new StreamReader(stream);

        var localidades = new List<Localidad>();
        bool primera = true;

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (primera)
            {
                primera = false;
                continue; // encabezado
            }

          
            var parts = line.Split(',');

            if (parts.Length < 5)
                continue; 

            localidades.Add(new Localidad
            {
                Cve_Ent = parts[0].Trim(),
                Cve_Mun = parts[1].Trim().PadLeft(3, '0'),     
                Cve_Loc = parts[2].Trim(),
                LocalidadNombre = parts[3].Trim(),
                Ambito = parts[4].Trim()
            });
        }

      
        await _database.InsertAllAsync(localidades);
    }


    public async Task<List<Localidad>> GetLocalidadesByCveMunAsync(String cveMun)
    {
        if (cveMun == "9999")
        {
            return new List<Localidad>
        {
            new Localidad
            {
                Cve_Mun = "9999",
                LocalidadNombre = "FUERA DEL ESTADO"
            }
        };
        }

        return await _database
            .Table<Localidad>()
            .Where(l => l.Cve_Mun == cveMun)
            .OrderBy(l => l.LocalidadNombre)
            .ToListAsync();
    }

    /*public async Task<List<string>> GetNombresLocalidadesByCveMunAsync(String cveMun)
    {
        var lista = await _database.Table<Localidad>()
            .Where(l => l.Cve_Mun == cveMun)
            .OrderBy(l => l.LocalidadNombre)
            .ToListAsync();

        return lista
            .Select(l => l.LocalidadNombre)
            .ToList();
    }*/
    public async Task<List<string>> GetNombresLocalidadesByCveMunAsync(string cveMun)
    {
        


        if (cveMun == "9999")
        {
            return new List<string>
        {
            "FUERA DEL ESTADO"
        };
        }

        var lista = await _database.Table<Localidad>()
            .Where(l => l.Cve_Mun == cveMun)
            .OrderBy(l => l.LocalidadNombre)
            .ToListAsync();

        return lista
            .Select(l => l.LocalidadNombre)
            .ToList();
    }


    public async Task<List<CodigoPostalCat>> GetTodosLosMunicipiosAsync()
    {
        var todos = await _database.Table<CodigoPostalCat>()
            .ToListAsync();
        var municipiosUnicos = todos
            .GroupBy(x => new { x.Municipio, x.cve_mun })
            .Select(g => g.First())
            .OrderBy(x => x.Municipio)
            .ToList();
        municipiosUnicos.Add(new CodigoPostalCat
        {
            Codigo = "00000",
            Colonia = "FUERA DEL ESTADO",
            Municipio = "FUERA DEL ESTADO",
            cve_mun = "9999",
            SeccionElectoral = "9999"
        });


        return municipiosUnicos;
    }


}