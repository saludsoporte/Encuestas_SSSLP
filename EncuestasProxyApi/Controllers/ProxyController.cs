using EncuestasProxyApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace EncuestasProxyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProxyController : ControllerBase
    {
        private readonly HttpClient _http;

        // URL del backend real (configurable en appsettings.json)
        private readonly string _backendUrl;

        // Verifica que la URL del backend esté configurada.
        public ProxyController(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("BackendApi");
            _backendUrl = config.GetValue<string>("BackendUrl") ?? throw new Exception("BackendUrl no configurado");
        }


        [HttpGet] // Verifica que el proxy esté funcionando.
        public async Task<IActionResult> Get()  
        {
            return Ok(new { Respuesta = "Proxy API está funcionando" });
        }

        [HttpPost("ping")] // Endpoint para probar conectividad con el backend real.
        public async Task<IActionResult> Ping([FromBody] string mensaje)
        {
            if (string.IsNullOrWhiteSpace(mensaje))
                return BadRequest("Mensaje vacío.");

            var response = await _http.PostAsJsonAsync($"{_backendUrl}/api/Encuestas/echo", mensaje);
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpPost("bulk")] // Recibe una lista de encuestas y las reenvía al backend real.
        public async Task<IActionResult> PostBulk([FromBody] List<EncuestaDto> encuestas)
        {
            try
            {
                if (encuestas == null || encuestas.Count == 0)
                    return BadRequest("No se enviaron encuestas");

                // ✅ 1. Convertir el modelo a JSON legible
                string jsonContent = JsonSerializer.Serialize(encuestas, new JsonSerializerOptions
                {
                    WriteIndented = true 
                });

                // ✅ 2. Definir la ruta del archivo donde se guardará
                string folderPath = Path.Combine(AppContext.BaseDirectory, "Logs");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                // ✅ 3. Crear un archivo nuevo con timestamp
                string fileName = $"Encuestas_{DateTime.Now:yyyyMMdd_HHmmss_fff}.txt";
                string filePath = Path.Combine(folderPath, fileName);

                // ✅ 4. Escribir el contenido en el archivo
                await System.IO.File.WriteAllTextAsync(filePath, jsonContent, Encoding.UTF8);

                // ✅ 5. (Opcional) También registrar con Serilog
                Log.Information("Encuestas recibidas guardadas en {FilePath}", filePath);

                // Reenviar al backend real
                var response = await _http.PostAsJsonAsync($"{_backendUrl}/api/Encuestas/bulk", encuestas);

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());

                var resultados = await response.Content.ReadAsStringAsync();
                return Content(resultados, "application/json");
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}