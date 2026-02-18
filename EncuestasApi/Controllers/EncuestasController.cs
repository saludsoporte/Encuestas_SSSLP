using EncuestasApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class EncuestasController : ControllerBase
{
    private readonly AppDbContext _db;
    private static readonly List<Encuesta> _encuestas = new();
    public EncuestasController(AppDbContext db) => _db = db;

    [HttpGet] // Prueba conexión con la BD listando las encuestas.
    public async Task<IActionResult> Get()
    {
        var encuestas = await _db.Encuestas.OrderBy(e => e.Id)
            .ToListAsync();

        return Ok(encuestas);
    }

    [HttpPost("echo")] // Prueba de conexión con EncuestasProxyApi.
    public IActionResult Echo([FromBody] string mensaje)
    {
        if (string.IsNullOrWhiteSpace(mensaje))
            return BadRequest("Mensaje vacío.");

            return Ok($"Backend recibió: {mensaje}");
    }

    [HttpPost("bulk")] // Inserción masiva de encuestas.
    public async Task<IActionResult> PostBulk([FromBody] List<EncuestaDto> encuestasDto)
    {
        if (encuestasDto == null || !encuestasDto.Any())
            return BadRequest("No se enviaron encuestas");

        // Validar DeviceId (se asume que todas las encuestas traen el mismo DeviceId)
        string deviceId = encuestasDto.First().DeviceId;

        var dispositivo = await _db.Dispositivos.FirstOrDefaultAsync(d => d.DeviceId == deviceId && d.Activo);

        if (dispositivo == null)
            return Unauthorized("Dispositivo no autorizado");

        var resultados = new List<SyncResult>();

        foreach (var dto in encuestasDto)
        {
            try
            {
                var existeEncuesta = await _db.EncuestasRespondidas.FirstOrDefaultAsync(x => x.EncuestaId == 1 && x.LocalId == dto.Id && x.DispositivoId == dispositivo.Id);

                if(existeEncuesta is not null)
                {
                    resultados.Add(new SyncResult
                    {
                        LocalId = dto.Id,
                        Success = true,
                        ServerId = existeEncuesta.Id,
                        Error = ""
                    });
                    continue; // Saltar a la siguiente encuesta
                }

                // Crear nueva encuesta respondida
                var encuestaRespondida = new EncuestasRespondida
                {
                    EncuestaId = 1,
                    Curp = dto.Curp.ToUpper(),
                    FechaInicio = DateTime.SpecifyKind(dto.FechaEncuesta, DateTimeKind.Utc),
                    FechaFin = DateTime.SpecifyKind(dto.FechaEncuesta, DateTimeKind.Utc),
                    Latitud = dto.Latitud,
                    Longitud = dto.Longitud,
                    Terminada = true,
                    Sincronizada = true,
                    UsuarioId = dto.UsuarioId,
                    FechaSincronizacion = DateTime.Now,
                    LocalId = dto.Id,
                    DispositivoId = dispositivo.Id
                };
                _db.EncuestasRespondidas.Add(encuestaRespondida);
                await _db.SaveChangesAsync();

                // ---- 1. Datos Generales del Paciente ----
                // Nombre completo
                var respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 1,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.NombreCompleto.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Edad
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 2,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Edad.ToString()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Género
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 6,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Genero.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Fecha de nacimiento
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 7,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.FechaNacimiento.ToString("yyyy-MM-dd")
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // CURP
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 8,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Curp.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Sección electoral
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 9,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.SeccionElectoral
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Calle
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 10,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Calle.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Número exterior
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 11,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.NumExterior.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Número interior
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 12,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.NumInterior ?? "").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Colonia
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 13,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Colonia.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Localidad
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 14,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Localidad.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Municipio
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 15,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Municipio.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Código Postal
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 16,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.CodigoPostal
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Teléfono de casa
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 17,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Telefono
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Teléfono celular
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 18,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Celular
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Estado civil
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 19,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.EstadoCivil.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Ocupación
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 20,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Ocupacion.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // ¿Pertenece a algún pueblo indígena? 
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 21,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.PuebloIndigena.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // ¿Cuál?:
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 9,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.CualPuebloIndigena ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // ¿Habla algún idioma indígena? 
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 22,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.IdiomaIndigena.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // ¿Cuál?:
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 11,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.CualIdiomaIndigena ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Acompañante o familiar responsable
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 23,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.Acompanante ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Relación
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 24,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.RelacionAcompanante ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Relación
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 25,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.TelefonoAcompanante ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // ---- 2. Antecedentes Personales ----
                // Hipertensión Arterial
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 26,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.HipertensionArterial == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Diabetes Mellitus
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 27,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Diabetes == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Obesidad
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 28,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Obesidad == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Enfermedad cardiovascular
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 29,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.EnfermedadCerdiovascular == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Dislipidemia (Colesterol/Trigliceridos)
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 30,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Dislipidemia == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Asma/EPOC
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 31,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Asma == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Tuberculosis
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 32,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Tuberculosis == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Epilepsia
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 33,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Epilepsia == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Cáncer
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 34,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Cancer == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Especificar Cáncer
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 13,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.EspecificaCancer ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Trastorno de salud mental (ansiedad, depresión, etc.)
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 35,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.SaludMental == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Especificar Trastorno de salud mental
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 14,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.EspecificaSaludMental ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Caries
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 36,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Caries == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Alergias
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 37,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Alergias == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Especificar Alergias
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 15,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.EspecificaAlergias ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Cirugías previas
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 38,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.CirugiasPrevias == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Especificar Cirugías previas
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 16,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.EspecificaCirugiasPrevias ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Infecciones de Transmisión sexual
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 39,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.InfeccionesTransmisionSexual == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Especificar Infecciones de Transmisión sexual
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 17,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.EspecificaInfeccionesTransmisionSexual ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Medicamentos actuales
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 18,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.MedicamentosActuales ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Otros antecedentes relevantes
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 40,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.OtrosAntecedentes == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Especificar Otros antecedentes relevantes
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 19,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.EspecificaOtrosAntecedentes ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Ginecológicos
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 41,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Ginecologicos == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Menarca
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 20,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.Menarca ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Inicio de vida sexual activa
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 21,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.InicioVidaSexual ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // No. de Parejas Sexuales
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 22,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ParejasSexuales ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Gestas
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 23,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.Gestas ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Partos
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 24,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.Partos ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Cesáreas
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 25,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.Cesareas ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Abortos
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 26,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.Abortos ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Citología Cervical
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 27,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.CitologiaCervical ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Exploración Mamaria
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 28,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ExploracionMamariaObservaciones ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Mastografía
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 29,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.Mastografia ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Método de Planificación Familiar
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 30,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.PlanificacionFamiliar ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // ---- 3. Antecedentes Familiares ----
                // Hipertensión Arterial
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 42,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.AntecedentesHipertension == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Parentesco
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 31,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ParentescoHipertension ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Diabetes Mellitus
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 44,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.AntecedentesDiabetes == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Parentesco
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 38,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ParentescoDiabetes ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Obesidad
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 45,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.AntecedentesObesidadCheckBox == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Parentesco
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 43,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ParentescoObesidad ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Enfermedad cardiovascular
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 46,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.AntecedentesEnfermedadCardiovascular == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Parentesco
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 48,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ParentescoEnfermedadCardiovascular ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Dislipidemia (Colesterol/Triglicéridos)
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 47,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.AntecedentesDislipidemia == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Parentesco
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 53,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ParentescoDislipidemian ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Asma/EPOC
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 48,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.AntecedentesAsma == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Parentesco
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 58,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ParentescoAsma ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Epilepsia
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 49,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.AntecedentesEpilepsia == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Parentesco
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 63,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ParentescoEpilepsia ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Cáncer
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 50,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.AntecedentesCancer == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Parentesco
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 68,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ParentesCancer ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Trastorno de salud mental (ansiedad, depresión, etc.)
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 51,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.AntecedentesCancer == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Parentesco
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 73,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ParentesCancer ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // ---- 4. Estilo de vida y factores de riesgo ----
                // Tabaquismo
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 52,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Tabaquismo.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Cigarrillos/dí
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 79,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.TabaquismoNum ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Consumo de alcohol
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 53,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.ConsumoAlcohol.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Frecuencia
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 81,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ConsumoAlcoholFrecuencia ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Consumo de drogas
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 54,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.ConsumoDrogas.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Frecuencia
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 83,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ConsumoDrogasFrecuencia ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Actividad física
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 55,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.AcividadFisica.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Alimentación
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 56,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Alimentacion.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Nivel de estrés
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 57,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.NivelEstres.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Apoyo familiar o red de apoyo
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 58,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.ApoyoFamiliar.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Cuenta con mascotas
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 59,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.TieneMascotas.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Están vacunados
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 96,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.EstanVacunados ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // ---- 5. Evaluación social y condiciones del entorno ----
                // Tipo de vivienda
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 60,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.TipoVivienda.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Condiciones del hogar
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 61,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.CondicionesHogar.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Acceso a servicios básicos
                // Agua
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 103,
                    SubopcionId = null,
                    DetalleRespuesta = dto.ServicioAgua == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Luz
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 104,
                    SubopcionId = null,
                    DetalleRespuesta = dto.ServicioLuz == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Gas
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 105,
                    SubopcionId = null,
                    DetalleRespuesta = dto.ServicioGas == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Drenaje
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 106,
                    SubopcionId = null,
                    DetalleRespuesta = dto.ServicioDrenaje == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // En su casa habitan
                // Personas mayores
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 107,
                    SubopcionId = null,
                    DetalleRespuesta = dto.PersonasMayores == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Personas con discapacidad
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 108,
                    SubopcionId = null,
                    DetalleRespuesta = dto.PersonasDiscapacidad == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Niños/as menores de 5 años
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 109,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Menores5anios == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Riesgos sociales identificados
                // Violencia intrafamiliar
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 110,
                    SubopcionId = null,
                    DetalleRespuesta = dto.ViolenciaFamiliar == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Abandono
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 111,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Abandono == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Inseguridad alimentaria
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 112,
                    SubopcionId = null,
                    DetalleRespuesta = dto.InseguridadAlimentaria == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Desempleo
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 113,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Desempleo == false ? "NO" : "SI"
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Observaciones
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 114,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.RiegosSocialesObservaciones ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // ---- 6. Motivo de consulta y padecimiento actual ----
                // (Descripción breve del motivo de consulta y problema de salud actual: síntomas, tiempo de evolución, tratamientos previos, etc.)
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 65,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.MotivoConsulta.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // ---- 7. Interrogatorio por Aparatos y Sistemas (breve) ----
                // Respiratorio
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 66,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Respiratorio.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Otro
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 117,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.EspecificaOtroRespiratorio ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Cardiovascular
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 67,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Cardiovascular.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Otro
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 120,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.EspecificaOtroCardiovascular ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Digestivo
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 68,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Digestivo.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Otro
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 123,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.EspecificaOtroDigestivo ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Musculoesquelético
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 68,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Musculoesqueletico.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Otro
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 123,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.EspecificaOtroMusculoesqueletico ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Neurológico
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 69,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Musculoesqueletico.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Otro
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 126,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.EspecificaOtroMusculoesqueletico ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Genitourinario
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 96,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Genitourinario.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Otro
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 182,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.EspecificaOtroGenitourinario ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Psicológico
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 71,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Psicologico.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Otro
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 133,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.EspecificaOtroPsicologico ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // ---- 8. Examen Físico ----
                // Signos Vitales:
                // o	Tensión arterial
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 134,
                    SubopcionId = null,
                    DetalleRespuesta = dto.TensionArterial.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // o	Frecuencia cardíaca
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 135,
                    SubopcionId = null,
                    DetalleRespuesta = dto.FrecuenciaCardiaca.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // o	Temperatura
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 136,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Temperatura.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // o	Frecuencia respiratoria
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 137,
                    SubopcionId = null,
                    DetalleRespuesta = dto.FrecuenciaRespiratoria.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // o	Saturación O₂
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 138,
                    SubopcionId = null,
                    DetalleRespuesta = dto.SaturacionO2.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // o	Peso
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 139,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Peso.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // o	Talla
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 140,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Talla.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // o	IMC
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 141,
                    SubopcionId = null,
                    DetalleRespuesta = dto.Imc.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 141,
                    SubopcionId = null,
                    DetalleRespuesta = dto.ImcPicker.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Estado general:
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 73,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.EstadoGeneral.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Exploración física:
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 74,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ExploracionFisica ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // ---- 9. Detecciones (aplicar según edad y género) ----
                // Tamizaje de hipertensión (TA elevada)
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 75,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.TamizajeHipertension.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Observaciones
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 147,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.TamizajeHipertensionObservaciones ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Tamizaje de diabetes (glucemia capilar)
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 76,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.TamizajeDiabetes.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Observaciones
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 150,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.TamizajeDiabetesObservaciones ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Exploración mamaria (≥20 años)
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 77,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.DeteccionesExploracionMamaria.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Observaciones
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 153,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.DeteccionesExploracionMamariaObservaciones ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Exploración clínica de mamaria
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 78,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.DeteccionesExploracionClinicaMamaria.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Observaciones
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 156,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.DeteccionesExploracionClinicaMamariaObservaciones ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Evaluación del estado nutricional (test breve anexo 1)
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 79,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.DeteccionesEvaluacionEstadoNutricional.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Observaciones
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 159,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.DeteccionesEvaluacionEstadoNutricionalObservaciones ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Salud mental (test breve anexo 2)
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 80,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.DeteccionesSaludMental.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Observaciones
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 162,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.DeteccionesSaludMentalObservaciones ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Revisión Bucal
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 81,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.DeteccionesRevisionBucal.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Observaciones
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 165,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.DeteccionesRevisionBucalObservaciones ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // ---- 10. Impresión Diagnóstica / Problemas Identificados ----
                // 1.
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 83,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ImpresionDiagnostica1 ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // 2.
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 86,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ImpresionDiagnostica2 ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // 3.
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 87,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.ImpresionDiagnostica3 ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // ---- 11. Plan de Manejo ----
                // Tratamiento / Medicamentos entregados:
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 88,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.TratamientoMedicamentosEntregados ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Entrega de suplementos / insumos:
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 89,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.EntregaSuplementosInsumos ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Educación en salud:
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 90,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.EducacionSalud ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Referencias necesarias:
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 91,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.ReferenciasNecesarias.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Lugar
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 169,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.Lugar ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Referencias necesarias:
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 91,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.ReferenciasNecesarias.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Lugar
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 169,
                    SubopcionId = null,
                    DetalleRespuesta = (dto.Lugar ?? "NINGUNO").ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Visita de seguimiento:
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 92,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.VisitaSeguimiento.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Fecha sugerida
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = null,
                    OpcionId = 172,
                    SubopcionId = null,
                    DetalleRespuesta = dto.FechaSugerida.ToString("yyyy-MM-dd")
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // ---- 12. Profesional que Realiza la Atención ----
                // Nombre completo
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 93,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.NombreEncuestador.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // Cargo
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 94,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.CargoEncuestador.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();

                // ¿Qué tanta disposición mostró la persona entrevistada hacía el brigadista?
                respuestas = new Respuesta
                {
                    EncuestaRespondidaId = encuestaRespondida.Id,
                    PreguntaId = 95,
                    OpcionId = null,
                    SubopcionId = null,
                    DetalleRespuesta = dto.DisposicionEncuestado.ToUpper()
                };
                _db.Respuestas.Add(respuestas);
                await _db.SaveChangesAsync();


                resultados.Add(new SyncResult { LocalId = dto.Id, Success = true, ServerId = encuestaRespondida.Id, Error = "" });
            }
            catch (Exception ex)
            {
                resultados.Add(new SyncResult { LocalId = dto.Id, Success = false, Error = ex.Message });
            }
        }

        return Ok(resultados);
    }
}