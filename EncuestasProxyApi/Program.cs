using Serilog;
using Microsoft.AspNetCore.Http.Timeouts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// --- Configuración de Serilog ---
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // Nivel mínimo de log a registrar
    .WriteTo.Console() // Escribe logs en la consola
    .WriteTo.File("logs/encuestasproxyapi-.log", rollingInterval: RollingInterval.Day) // Escribe en un archivo diario
    .CreateLogger();

// Usa Serilog como el proveedor de logging de la aplicación
builder.Host.UseSerilog();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 52428800; // 50 MB
});

// HttpClient para proxy
builder.Services.AddHttpClient("BackendApi", client =>
{
    client.Timeout = TimeSpan.FromMinutes(15);
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Agrega el middleware de Serilog para loguear información de las peticiones
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();