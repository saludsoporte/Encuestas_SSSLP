using EncuestaApp.Services;
using EncuestaApp.Models;
using Microsoft.Maui.Storage;

namespace EncuestaApp.Views;

public partial class LoginPage : ContentPage
{
    private readonly DatabaseService _db;

    private void OnTogglePasswordClicked(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

        TogglePasswordButton.Source = new FontImageSource
        {
            Glyph = PasswordEntry.IsPassword ? "\ue8f5" : "\ue417",
            FontFamily = "MaterialIcons",
            Size = 22,
            Color = Application.Current.RequestedTheme == AppTheme.Dark
                ? Colors.White
                : Colors.Black
        };
    }

    public LoginPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
        _db = new DatabaseService(Path.Combine(FileSystem.AppDataDirectory, "encuestas.db3"));
        _ = SeedUsuarios(); // opcional: poblar un usuario por defecto

        // Obtiene la versión y la muestra
        string version = AppInfo.Current.VersionString; // Ejemplo: "1.2.3"
        lblVersion.Text = $"Versión {version}";
    }

    //  Insertar usuario inicial si no existe
    private async Task SeedUsuarios()
    {//$_S@1u4App2@25
        var existente = await _db.GetUsuarioAsync("administrador", "S@1ud");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "administrador",
                Password = "S@1ud",
                NombreCompleto = "Administrador"
            });
        }
         existente = await _db.GetUsuarioAsync("admin", "1234");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "admin",
                Password = "1234",
                NombreCompleto = "Administrador"
            });
        }
        existente = await _db.GetUsuarioAsync("administrador", "S@1ud");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "administrador",
                Password = "S@1ud",
                NombreCompleto = "Administrador"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada1", "TQkN6F");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada1",
                Password = "TQkN6F",
                NombreCompleto = "Brigada 1"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada2", "GU4GeY");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada2",
                Password = "GU4GeY",
                NombreCompleto = "Brigada 2"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada3", "ebOV0N");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada3",
                Password = "ebOV0N",
                NombreCompleto = "Brigada 3"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada4", "B5iccM");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada4",
                Password = "B5iccM",
                NombreCompleto = "Brigada 4"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada5", "EAbUs2");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada5",
                Password = "EAbUs2",
                NombreCompleto = "Brigada 5"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada6", "HC1oY2");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada6",
                Password = "HC1oY2",
                NombreCompleto = "Brigada 6"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada7", "6PAxwm");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada7",
                Password = "6PAxwm",
                NombreCompleto = "Brigada 7"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada8", "u2dO9x");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada8",
                Password = "u2dO9x",
                NombreCompleto = "Brigada 8"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada9", "1PaSE6");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada9",
                Password = "1PaSE6",
                NombreCompleto = "Brigada 9"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada10", "3M3HIm");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada10",
                Password = "3M3HIm",
                NombreCompleto = "Brigada 10"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada11", "6bFTO6");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada11",
                Password = "6bFTO6",
                NombreCompleto = "Brigada 11"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada12", "76iCAd");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada12",
                Password = "76iCAd",
                NombreCompleto = "Brigada 12"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada13", "ubNPF0");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada13",
                Password = "ubNPF0",
                NombreCompleto = "Brigada 13"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada14", "07mAd8");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada14",
                Password = "07mAd8",
                NombreCompleto = "Brigada 14"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada15", "8jC4Xa");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada15",
                Password = "8jC4Xa",
                NombreCompleto = "Brigada 15"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada16", "u2uGwu");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada16",
                Password = "u2uGwu",
                NombreCompleto = "Brigada 16"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada17", "X56ccY");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada17",
                Password = "X56ccY",
                NombreCompleto = "Brigada 17"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada18", "CFdC8N");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada18",
                Password = "CFdC8N",
                NombreCompleto = "Brigada 18"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada19", "1nMlYr");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada19",
                Password = "1nMlYr",
                NombreCompleto = "Brigada 19"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada20", "sS9LuI");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada20",
                Password = "sS9LuI",
                NombreCompleto = "Brigada 20"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada21", "gV0AM");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada21",
                Password = "gV0AM",
                NombreCompleto = "Brigada 21"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada22", "5Ceub");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada22",
                Password = "5Ceub",
                NombreCompleto = "Brigada 22"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada23", "5eWx9");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada23",
                Password = "5eWx9",
                NombreCompleto = "Brigada 23"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada24", "j4Fm3");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada24",
                Password = "j4Fm3",
                NombreCompleto = "Brigada 24"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada25", "laW5w");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada25",
                Password = "laW5w",
                NombreCompleto = "Brigada 25"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada26", "Eh4nP");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada26",
                Password = "Eh4nP",
                NombreCompleto = "Brigada 26"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada27", "i0LuZ");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada27",
                Password = "i0LuZ",
                NombreCompleto = "Brigada 27"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada28", "pIX5R");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada28",
                Password = "pIX5R",
                NombreCompleto = "Brigada 28"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada29", "zHRj0");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada29",
                Password = "zHRj0",
                NombreCompleto = "Brigada 29"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada30", "cZX4s");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada30",
                Password = "cZX4s",
                NombreCompleto = "Brigada 30"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada31", "db81N");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada31",
                Password = "db81N",
                NombreCompleto = "Brigada 31"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada32", "80RrB");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada32",
                Password = "80RrB",
                NombreCompleto = "Brigada 32"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada33", "nYR3h");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada33",
                Password = "nYR3h",
                NombreCompleto = "Brigada 33"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada34", "GBG0v");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada34",
                Password = "GBG0v",
                NombreCompleto = "Brigada 34"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada35", "74NsH");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada35",
                Password = "74NsH",
                NombreCompleto = "Brigada 35"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada36", "44Fif");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada36",
                Password = "44Fif",
                NombreCompleto = "Brigada 36"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada37", "C0iBL");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada37",
                Password = "C0iBL",
                NombreCompleto = "Brigada 37"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada38", "5Ph6V");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada38",
                Password = "5Ph6V",
                NombreCompleto = "Brigada 38"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada39", "4kP9u");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada39",
                Password = "4kP9u",
                NombreCompleto = "Brigada 39"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada40", "iC1kE");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada40",
                Password = "iC1kE",
                NombreCompleto = "Brigada 40"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada41", "Ts6EG");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada41",
                Password = "Ts6EG",
                NombreCompleto = "Brigada 41"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada42", "41FsD");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada42",
                Password = "41FsD",
                NombreCompleto = "Brigada 42"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada43", "BO8Pb");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada43",
                Password = "BO8Pb",
                NombreCompleto = "Brigada 43"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada44", "Wt2IY");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada44",
                Password = "Wt2IY",
                NombreCompleto = "Brigada 44"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada44", "Wt2IY");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada44",
                Password = "Wt2IY",
                NombreCompleto = "Brigada 44"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada45", "y9Kjp");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada45",
                Password = "y9Kjp",
                NombreCompleto = "Brigada 45"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada46", "ZI6Ui");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada46",
                Password = "ZI6Ui",
                NombreCompleto = "Brigada 46"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada47", "t2Wiw");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada47",
                Password = "t2Wiw",
                NombreCompleto = "Brigada 47"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada48", "0VlFY");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada48",
                Password = "0VlFY",
                NombreCompleto = "Brigada 48"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada49", "Mga6O");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada49",
                Password = "Mga6O",
                NombreCompleto = "Brigada 49"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada50", "V5ki1");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada50",
                Password = "V5ki1",
                NombreCompleto = "Brigada 50"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada51", "vfhFT");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada51",
                Password = "vfhFT",
                NombreCompleto = "Brigada 51"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada52", "LeIgs");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada52",
                Password = "LeIgs",
                NombreCompleto = "Brigada 52"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada53", "KuDYa");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada53",
                Password = "KuDYa",
                NombreCompleto = "Brigada 53"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada54", "ZVvDx");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada54",
                Password = "ZVvDx",
                NombreCompleto = "Brigada 54"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada55", "QPopR");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada55",
                Password = "QPopR",
                NombreCompleto = "Brigada 55"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada56", "s1sxc");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada56",
                Password = "s1sxc",
                NombreCompleto = "Brigada 56"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada57", "Z0fup");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada57",
                Password = "Z0fup",
                NombreCompleto = "Brigada 57"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada58", "SZmqC");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada58",
                Password = "SZmqC",
                NombreCompleto = "Brigada 58"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada59", "6nRFX");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada59",
                Password = "6nRFX",
                NombreCompleto = "Brigada 59"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada60", "KpdNd");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada60",
                Password = "KpdNd",
                NombreCompleto = "Brigada 60"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada61", "CLXwY");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada61",
                Password = "CLXwY",
                NombreCompleto = "Brigada 61"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada62", "p1VaW");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada62",
                Password = "p1VaW",
                NombreCompleto = "Brigada 62"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada63", "P0nHN");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada63",
                Password = "P0nHN",
                NombreCompleto = "Brigada 63"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada64", "RQuEO");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada64",
                Password = "RQuEO",
                NombreCompleto = "Brigada 64"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada65", "H0sCe");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada65",
                Password = "H0sCe",
                NombreCompleto = "Brigada 65"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada66", "bXiqb");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada66",
                Password = "bXiqb",
                NombreCompleto = "Brigada 66"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada67", "cxKVb");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada67",
                Password = "cxKVb",
                NombreCompleto = "Brigada 67"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada68", "tryiS");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada68",
                Password = "tryiS",
                NombreCompleto = "Brigada 68"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada69", "GGj1H");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada69",
                Password = "GGj1H",
                NombreCompleto = "Brigada 69"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada70", "QScWY");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada70",
                Password = "QScWY",
                NombreCompleto = "Brigada 70"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada71", "69tih");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada71",
                Password = "69tih",
                NombreCompleto = "Brigada 71"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada72", "CaB4B");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada72",
                Password = "CaB4B",
                NombreCompleto = "Brigada 72"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada73", "yTFyE");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada73",
                Password = "yTFyE",
                NombreCompleto = "Brigada 73"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada74", "DoKtO");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada74",
                Password = "DoKtO",
                NombreCompleto = "Brigada 74"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada75", "hq5Gp");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada75",
                Password = "hq5Gp",
                NombreCompleto = "Brigada 75"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada76", "JKjLt");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada76",
                Password = "JKjLt",
                NombreCompleto = "Brigada 76"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada77", "5rjcf");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada77",
                Password = "5rjcf",
                NombreCompleto = "Brigada 77"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada78", "0BWk3");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada78",
                Password = "0BWk3",
                NombreCompleto = "Brigada 78"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada79", "K49w0");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada79",
                Password = "K49w0",
                NombreCompleto = "Brigada 79"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada80", "32WiY");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada80",
                Password = "32WiY",
                NombreCompleto = "Brigada 80"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada81", "NvDGh");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada81",
                Password = "NvDGh",
                NombreCompleto = "Brigada 81"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada82", "p1cRs");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada82",
                Password = "p1cRs",
                NombreCompleto = "Brigada 82"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada83", "j8cyy");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada83",
                Password = "j8cyy",
                NombreCompleto = "Brigada 83"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada84", "9i4Vj");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada84",
                Password = "9i4Vj",
                NombreCompleto = "Brigada 84"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada85", "4tgXt");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada85",
                Password = "4tgXt",
                NombreCompleto = "Brigada 85"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada86", "kSkAK");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada86",
                Password = "kSkAK",
                NombreCompleto = "Brigada 86"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada87", "KGbF9");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada87",
                Password = "KGbF9",
                NombreCompleto = "Brigada 87"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada88", "oarzV");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada88",
                Password = "oarzV",
                NombreCompleto = "Brigada 88"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada89", "EYbvP");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada89",
                Password = "EYbvP",
                NombreCompleto = "Brigada 89"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada90", "Ff36W");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada90",
                Password = "Ff36W",
                NombreCompleto = "Brigada 90"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada91", "zzB7T");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada91",
                Password = "zzB7T",
                NombreCompleto = "Brigada 91"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada92", "vdi23");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada92",
                Password = "vdi23",
                NombreCompleto = "Brigada 92"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada93", "3wxba");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada93",
                Password = "3wxba",
                NombreCompleto = "Brigada 93"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada94", "0tPV2");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada94",
                Password = "0tPV2",
                NombreCompleto = "Brigada 94"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada95", "XHAxG");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada95",
                Password = "XHAxG",
                NombreCompleto = "Brigada 95"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada96", "wn8qh");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada96",
                Password = "wn8qh",
                NombreCompleto = "Brigada 96"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada97", "0iEX8");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada97",
                Password = "0iEX8",
                NombreCompleto = "Brigada 97"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada98", "NjlBa");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada98",
                Password = "NjlBa",
                NombreCompleto = "Brigada 98"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada99", "8KSPr");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada99",
                Password = "8KSPr",
                NombreCompleto = "Brigada 99"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada100", "pzmup");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada100",
                Password = "pzmup",
                NombreCompleto = "Brigada 100"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada101", "JNqCh");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada101",
                Password = "JNqCh",
                NombreCompleto = "Brigada 101"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada102", "XGyTm");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada102",
                Password = "XGyTm",
                NombreCompleto = "Brigada 102"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada103", "1WQd8");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada103",
                Password = "1WQd8",
                NombreCompleto = "Brigada 103"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada104", "ifZzJ");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada104",
                Password = "ifZzJ",
                NombreCompleto = "Brigada 104"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada105", "xCIF2");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada105",
                Password = "xCIF2",
                NombreCompleto = "Brigada 105"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada106", "8nSHs");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada106",
                Password = "8nSHs",
                NombreCompleto = "Brigada 106"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada107", "XWYVs");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada107",
                Password = "XWYVs",
                NombreCompleto = "Brigada 107"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada108", "CzlnG");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada108",
                Password = "CzlnG",
                NombreCompleto = "Brigada 108"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada109", "0Hi61");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada109",
                Password = "0Hi61",
                NombreCompleto = "Brigada 109"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada110", "mqj8e");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada110",
                Password = "mqj8e",
                NombreCompleto = "Brigada 110"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada111", "TiOgM");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada111",
                Password = "TiOgM",
                NombreCompleto = "Brigada 111"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada112", "gbPjo");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada112",
                Password = "gbPjo",
                NombreCompleto = "Brigada 112"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada113", "SbQCz");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada113",
                Password = "SbQCz",
                NombreCompleto = "Brigada 113"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada114", "ubgLe");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada114",
                Password = "ubgLe",
                NombreCompleto = "Brigada 114"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada115", "tuc3f");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada115",
                Password = "tuc3f",
                NombreCompleto = "Brigada 115"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada116", "eap4r");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada116",
                Password = "eap4r",
                NombreCompleto = "Brigada 116"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada117", "rfaNk");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada117",
                Password = "rfaNk",
                NombreCompleto = "Brigada 117"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada118", "Gcw42");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada118",
                Password = "Gcw42",
                NombreCompleto = "Brigada 118"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada119", "THZlH");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada119",
                Password = "THZlH",
                NombreCompleto = "Brigada 119"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada120", "WKVen");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada120",
                Password = "WKVen",
                NombreCompleto = "Brigada 120"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada121", "UQtHc");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada121",
                Password = "UQtHc",
                NombreCompleto = "Brigada 121"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada122", "5IsVx");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada122",
                Password = "5IsVx",
                NombreCompleto = "Brigada 122"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada123", "hWxhD");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada123",
                Password = "hWxhD",
                NombreCompleto = "Brigada 123"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada124", "519Ur");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada124",
                Password = "519Ur",
                NombreCompleto = "Brigada 124"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada125", "bEBPa");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada125",
                Password = "bEBPa",
                NombreCompleto = "Brigada 125"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada126", "Vs3mA");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada126",
                Password = "Vs3mA",
                NombreCompleto = "Brigada 126"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada127", "7FBZc");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada127",
                Password = "7FBZc",
                NombreCompleto = "Brigada 127"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada128", "jkK33");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada128",
                Password = "jkK33",
                NombreCompleto = "Brigada 128"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada129", "I567P");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada129",
                Password = "I567P",
                NombreCompleto = "Brigada 129"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada130", "gRQng");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada130",
                Password = "gRQng",
                NombreCompleto = "Brigada 130"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada131", "WojHA");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada131",
                Password = "WojHA",
                NombreCompleto = "Brigada 131"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada132", "swxXj");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada132",
                Password = "swxXj",
                NombreCompleto = "Brigada 132"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada133", "QbrC4");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada133",
                Password = "QbrC4",
                NombreCompleto = "Brigada 133"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada134", "mBiTY");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada134",
                Password = "mBiTY",
                NombreCompleto = "Brigada 134"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada135", "Bjqx8");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada135",
                Password = "Bjqx8",
                NombreCompleto = "Brigada 135"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada136", "L6qzr");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada136",
                Password = "L6qzr",
                NombreCompleto = "Brigada 136"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada137", "09KJd");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada137",
                Password = "09KJd",
                NombreCompleto = "Brigada 137"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada138", "EC8Ri");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada138",
                Password = "EC8Ri",
                NombreCompleto = "Brigada 138"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada139", "CrgRJ");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada139",
                Password = "CrgRJ",
                NombreCompleto = "Brigada 139"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada140", "AqPBG");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada140",
                Password = "AqPBG",
                NombreCompleto = "Brigada 140"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada141", "syVc3");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada141",
                Password = "syVc3",
                NombreCompleto = "Brigada 141"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada142", "1oIF3");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada142",
                Password = "1oIF3",
                NombreCompleto = "Brigada 142"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada143", "ocPAh");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada143",
                Password = "ocPAh",
                NombreCompleto = "Brigada 143"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada144", "Gsc36");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada144",
                Password = "Gsc36",
                NombreCompleto = "Brigada 144"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada145", "PCWbT");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada145",
                Password = "PCWbT",
                NombreCompleto = "Brigada 145"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada146", "OIgc3");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada146",
                Password = "OIgc3",
                NombreCompleto = "Brigada 146"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada147", "1dA2Q");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada147",
                Password = "1dA2Q",
                NombreCompleto = "Brigada 147"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada148", "CZ9rM");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada148",
                Password = "CZ9rM",
                NombreCompleto = "Brigada 148"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada149", "DJw1Z");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada149",
                Password = "DJw1Z",
                NombreCompleto = "Brigada 149"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada150", "Olg9w");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada150",
                Password = "Olg9w",
                NombreCompleto = "Brigada 150"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada151", "idktS");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada151",
                Password = "idktS",
                NombreCompleto = "Brigada 151"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada152", "DVzBo");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada152",
                Password = "DVzBo",
                NombreCompleto = "Brigada 152"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada153", "cqiI0");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada153",
                Password = "cqiI0",
                NombreCompleto = "Brigada 153"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada154", "rcstB");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada154",
                Password = "rcstB",
                NombreCompleto = "Brigada 154"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada155", "4aSac");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada155",
                Password = "4aSac",
                NombreCompleto = "Brigada 155"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada156", "eN88c");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada156",
                Password = "eN88c",
                NombreCompleto = "Brigada 156"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada157", "wcJaO");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada157",
                Password = "wcJaO",
                NombreCompleto = "Brigada 157"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada158", "VWtkw");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada158",
                Password = "VWtkw",
                NombreCompleto = "Brigada 158"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada159", "dko1u");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada159",
                Password = "dko1u",
                NombreCompleto = "Brigada 159"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada160", "E8GZy");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada160",
                Password = "E8GZy",
                NombreCompleto = "Brigada 160"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada161", "idi4R");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada161",
                Password = "idi4R",
                NombreCompleto = "Brigada 161"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada162", "ryqNj");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada162",
                Password = "ryqNj",
                NombreCompleto = "Brigada 162"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada163", "kIBMe");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada163",
                Password = "kIBMe",
                NombreCompleto = "Brigada 163"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada164", "FVrF4");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada164",
                Password = "FVrF4",
                NombreCompleto = "Brigada 164"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada165", "Tq9zq");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada165",
                Password = "Tq9zq",
                NombreCompleto = "Brigada 165"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada166", "gzPxy");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada166",
                Password = "gzPxy",
                NombreCompleto = "Brigada 166"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada167", "bNRME");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada167",
                Password = "bNRME",
                NombreCompleto = "Brigada 167"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada168", "Vlcp8");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada168",
                Password = "Vlcp8",
                NombreCompleto = "Brigada 168"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada169", "LAll6");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada169",
                Password = "LAll6",
                NombreCompleto = "Brigada 169"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada170", "3Sbc8");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada170",
                Password = "3Sbc8",
                NombreCompleto = "Brigada 170"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada171", "6OPBi");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada171",
                Password = "6OPBi",
                NombreCompleto = "Brigada 171"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada172", "W761C");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada172",
                Password = "W761C",
                NombreCompleto = "Brigada 172"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada173", "dn67W");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada173",
                Password = "dn67W",
                NombreCompleto = "Brigada 173"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada174", "O9MsO");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada174",
                Password = "O9MsO",
                NombreCompleto = "Brigada 174"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada175", "Sghzq");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada175",
                Password = "Sghzq",
                NombreCompleto = "Brigada 175"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada176", "4zQ7U");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada176",
                Password = "4zQ7U",
                NombreCompleto = "Brigada 176"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada177", "tyVbH");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada177",
                Password = "tyVbH",
                NombreCompleto = "Brigada 177"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada178", "cRLPG");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada178",
                Password = "cRLPG",
                NombreCompleto = "Brigada 178"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada179", "zKjeC");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada179",
                Password = "zKjeC",
                NombreCompleto = "Brigada 179"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada180", "NO3HR");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada180",
                Password = "NO3HR",
                NombreCompleto = "Brigada 180"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada181", "JRDhv");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada181",
                Password = "JRDhv",
                NombreCompleto = "Brigada 181"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada182", "zwoOy");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada182",
                Password = "zwoOy",
                NombreCompleto = "Brigada 182"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada183", "bOIFs");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada183",
                Password = "bOIFs",
                NombreCompleto = "Brigada 183"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada184", "dQmrF");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada184",
                Password = "dQmrF",
                NombreCompleto = "Brigada 184"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada185", "ipFei");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada185",
                Password = "ipFei",
                NombreCompleto = "Brigada 185"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada186", "AZgir");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada186",
                Password = "AZgir",
                NombreCompleto = "Brigada 186"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada187", "qGa0Z");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada187",
                Password = "qGa0Z",
                NombreCompleto = "Brigada 187"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada188", "WR1WP");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada188",
                Password = "WR1WP",
                NombreCompleto = "Brigada 188"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada189", "AA0QG");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada189",
                Password = "AA0QG",
                NombreCompleto = "Brigada 189"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada190", "vDrcA");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada190",
                Password = "vDrcA",
                NombreCompleto = "Brigada 190"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada191", "ZsLbg");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada191",
                Password = "ZsLbg",
                NombreCompleto = "Brigada 191"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada192", "TyLfD");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada192",
                Password = "TyLfD",
                NombreCompleto = "Brigada 192"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada193", "yCOtC");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada193",
                Password = "yCOtC",
                NombreCompleto = "Brigada 193"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada194", "N3xTZ");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada194",
                Password = "N3xTZ",
                NombreCompleto = "Brigada 194"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada195", "G33aw");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada195",
                Password = "G33aw",
                NombreCompleto = "Brigada 195"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada196", "ALcGu");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada196",
                Password = "ALcGu",
                NombreCompleto = "Brigada 196"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada197", "PokAJ");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada197",
                Password = "PokAJ",
                NombreCompleto = "Brigada 197"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada198", "CbMmS");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada198",
                Password = "CbMmS",
                NombreCompleto = "Brigada 198"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada199", "KB9nF");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada199",
                Password = "KB9nF",
                NombreCompleto = "Brigada 199"
            });
        }

        existente = await _db.GetUsuarioAsync("brigada200", "Iqwc8");
        if (existente == null)
        {
            await _db.SaveUsuarioAsync(new Usuario
            {
                Username = "brigada200",
                Password = "Iqwc8",
                NombreCompleto = "Brigada 200"
            });
        }


    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string usuario = UserEntry.Text?.Trim() ?? "";
        string password = PasswordEntry.Text ?? "";

        // 🔑 Credenciales
        var user = await _db.GetUsuarioAsync(usuario, password);
        if (user != null)
        {
            App.UsuarioLogueadoId = user.Id; // 🔹 Guardamos el Id

            await DisplayAlert("Bienvenido", $"Bienvenido(a) {user.NombreCompleto} ✅", "OK");

            await Navigation.PushAsync(new EncuestasListPage(_db));
        }
        else
        {
            await DisplayAlert("Error", "Usuario o contraseña incorrectos ❌", "OK");
        }
    }
}