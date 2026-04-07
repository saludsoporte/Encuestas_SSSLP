using EncuestasApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class EncuestasController : ControllerBase
{
    private readonly AppDbContext _db;

    public EncuestasController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var encuestas = await _db.Encuestas.OrderBy(e => e.IdEncuesta).ToListAsync();
        return Ok(encuestas);
    }

    [HttpPost("echo")]
    public IActionResult Echo([FromBody] string mensaje)
    {
        if (string.IsNullOrWhiteSpace(mensaje))
            return BadRequest("Mensaje vacío.");

        return Ok($"Backend recibió: {mensaje}");
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> PostBulk([FromBody] List<EncuestaDto> encuestasDto)
    {
        if (encuestasDto == null || !encuestasDto.Any())
            return BadRequest("No se enviaron encuestas.");

        var resultados = new List<SyncResult>();

        foreach (var dto in encuestasDto)
        {
            // ── 1. Resolver Usuario ──────────────────────────────────────
            var usuario = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == dto.Usuario && u.Activo == true);

            if (usuario == null)
            {
                resultados.Add(new SyncResult
                {
                    LocalId = dto.id,
                    Success = false,
                    Error = $"[CURP: {dto.Curp}] Usuario '{dto.Usuario}' no encontrado o inactivo."
                });
                continue;
            }

            // ── 2. Resolver Dispositivo ──────────────────────────────────
            var dispositivo = await _db.DispositivosAutorizados
                .FirstOrDefaultAsync(d => d.Identificador == dto.Identificador && d.Activo == true);

            if (dispositivo == null)
            {
                resultados.Add(new SyncResult
                {
                    LocalId = dto.id,
                    Success = false,
                    Error = $"[CURP: {dto.Curp}] Dispositivo no encontrado o inactivo."
                });
                continue;
            }

            // ── 3. Resolver Versión ──────────────────────────────────────
            var version = await _db.VersionesEncuesta
                .FirstOrDefaultAsync(v => v.Descripcion == dto.Version && v.Activa == true);

            if (version == null)
            {
                resultados.Add(new SyncResult
                {
                    LocalId = dto.id,
                    Success = false,
                    Error = $"[CURP: {dto.Curp}] Versión '{dto.Version}' no encontrada o inactiva."
                });
                continue;
            }

            // ── 4. Verificar duplicado ───────────────────────────────────
            var existente = await _db.Encuestas
                .FirstOrDefaultAsync(e =>
                    e.Curp == dto.Curp.ToUpper() &&
                    e.IdUsuario == usuario.IdUsuario &&
                    e.IdDispositivo == dispositivo.IdDispositivo &&
                    e.IdVersion == version.IdVersion && 
                    e.FechaCaptura == dto.Fecha_Captura &&
                    e.Seccion1.Equals(dto.Seccion1));
            /*
            if (existente != null)
            {
                resultados.Add(new SyncResult
                {
                    LocalId = dto.id,
                    Success = true,
                    ServerId = existente.IdEncuesta,
                    Error = $"[CURP: {dto.Curp}] Encuesta ya existente, omitida (id_encuesta: {existente.IdEncuesta})."
                });
                continue;
            }*/

            // ── 5. Insertar ──────────────────────────────────────────────
            try
            {
                var encuesta = new Encuesta
                {
                    IdUsuario = usuario.IdUsuario,
                    IdDispositivo = dispositivo.IdDispositivo,
                    IdVersion = version.IdVersion,
                    Curp = dto.Curp.ToUpper(),
                    FechaCaptura = dto.Fecha_Captura,
                    FechaSincronizacion = DateTime.UtcNow,
                    Latitud = dto.Latitud,
                    Longitud = dto.Longitud,
                    PuertaAbierta = dto.Abrio_puerta,
                    ObsPuerta = dto.ObsPuerta,
                    Participa = dto.Acepto_encuesta,
                    ObsParticipa = dto.ObsParticipa,
                    Seccion1 = dto.Seccion1.HasValue ? JsonDocument.Parse(dto.Seccion1.Value.GetRawText()) : null,
                    Seccion2 = dto.Seccion2.HasValue ? JsonDocument.Parse(dto.Seccion2.Value.GetRawText()) : null,
                    Seccion3 = dto.Seccion3.HasValue ? JsonDocument.Parse(dto.Seccion3.Value.GetRawText()) : null,
                    Seccion4 = dto.Seccion4.HasValue ? JsonDocument.Parse(dto.Seccion4.Value.GetRawText()) : null,
                    Seccion5 = dto.Seccion5.HasValue ? JsonDocument.Parse(dto.Seccion5.Value.GetRawText()) : null,
                    Seccion6 = dto.Seccion6.HasValue ? JsonDocument.Parse(dto.Seccion6.Value.GetRawText()) : null,
                    Seccion7 = dto.Seccion7.HasValue ? JsonDocument.Parse(dto.Seccion7.Value.GetRawText()) : null,
                    Seccion8 = dto.Seccion8.HasValue ? JsonDocument.Parse(dto.Seccion8.Value.GetRawText()) : null,
                    Seccion9 = dto.Seccion9.HasValue ? JsonDocument.Parse(dto.Seccion9.Value.GetRawText()) : null,
                    Seccion10 = dto.Seccion10.HasValue ? JsonDocument.Parse(dto.Seccion10.Value.GetRawText()) : null,
                    Seccion11 = dto.Seccion11.HasValue ? JsonDocument.Parse(dto.Seccion11.Value.GetRawText()) : null,
                    Seccion12 = dto.Seccion12.HasValue ? JsonDocument.Parse(dto.Seccion12.Value.GetRawText()) : null,
                    Seccion13 = dto.Seccion13.HasValue ? JsonDocument.Parse(dto.Seccion13.Value.GetRawText()) : null,
                };

                _db.Encuestas.Add(encuesta);
                await _db.SaveChangesAsync();

                resultados.Add(new SyncResult
                {
                    LocalId = dto.id,
                    Success = true,
                    ServerId = encuesta.IdEncuesta,
                    Error = ""
                });
            }
            catch (Exception ex)
            {
                resultados.Add(new SyncResult
                {
                    LocalId = dto.id,
                    Success = false,
                    Error = $"[CURP: {dto.Curp}] Error al guardar: {ex.InnerException?.Message ?? ex.Message}"
                });
            }
        }

        // ── Resumen final ────────────────────────────────────────────────
        int exitosas = resultados.Count(r => r.Success && string.IsNullOrEmpty(r.Error));
        int omitidas = resultados.Count(r => r.Success && !string.IsNullOrEmpty(r.Error));
        int fallidas = resultados.Count(r => !r.Success);

        return Ok(new
        {
            Resumen = new
            {
                Total = resultados.Count,
                Exitosas = exitosas,
                Omitidas = omitidas,
                Fallidas = fallidas
            },
            Detalle = resultados
        });
    }
}