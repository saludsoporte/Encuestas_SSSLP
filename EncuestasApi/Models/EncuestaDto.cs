using System.Text.Json;
using System.Text.Json.Serialization;

public class EncuestaDto
{
    // Identificador local del dispositivo (para rastrear resultado)
    
    public int id { get; set; }

    // Claves de búsqueda en BD
    public string Usuario { get; set; } = string.Empty;
    public string Identificador { get; set; } = string.Empty;  
    public string Version { get; set; } = string.Empty;

    // Datos directos
    public string Curp { get; set; } = string.Empty;
    public DateTime Fecha_Captura { get; set; }
    public string? Latitud { get; set; }
    public string? Longitud { get; set; }
    public bool? Abrio_puerta { get; set; }
    public string? ObsPuerta { get; set; }
    public bool? Acepto_encuesta { get; set; }
    public string? ObsParticipa { get; set; }

    // Secciones JSONB
    public JsonElement? Seccion1 { get; set; }
    public JsonElement? Seccion2 { get; set; }
    public JsonElement? Seccion3 { get; set; }
    public JsonElement? Seccion4 { get; set; }
    public JsonElement? Seccion5 { get; set; }
    public JsonElement? Seccion6 { get; set; }
    public JsonElement? Seccion7 { get; set; }
    public JsonElement? Seccion8 { get; set; }
    public JsonElement? Seccion9 { get; set; }
    public JsonElement? Seccion10 { get; set; }
    public JsonElement? Seccion11 { get; set; }
    public JsonElement? Seccion12 { get; set; }
    public JsonElement? Seccion13 { get; set; }
}