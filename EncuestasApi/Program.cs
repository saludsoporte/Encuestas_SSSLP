using EncuestasApi.Models;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// --- Configuración de Serilog ---
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // Nivel mínimo de log a registrar
    .WriteTo.Console() // Escribe logs en la consola
    .WriteTo.File("logs/encuestasapi-.log", rollingInterval: RollingInterval.Day) // Escribe en un archivo diario
    .CreateLogger();

// Usa Serilog como el proveedor de logging de la aplicación
builder.Host.UseSerilog();

// PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRequestTimeouts(options => {
    options.DefaultPolicy =
        new RequestTimeoutPolicy
        {
            Timeout = TimeSpan.FromMilliseconds(15000000),
            TimeoutStatusCode = StatusCodes.Status503ServiceUnavailable
        };
});

builder.Services.AddControllers();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes("supersecretkey12345"))
        };
    });

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

app.UseRequestTimeouts();

app.Run();