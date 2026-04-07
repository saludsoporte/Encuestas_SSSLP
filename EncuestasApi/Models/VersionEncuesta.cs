namespace EncuestasApi.Models;
public class VersionEncuesta
{
    public int IdVersion { get; set; }
    public string? Descripcion { get; set; }
    public bool? Activa { get; set; }
    public DateOnly? FechaCreacion { get; set; }

    public ICollection<Encuesta> Encuestas { get; set; } = new List<Encuesta>();
    public ICollection<PreguntaVersion> Preguntas { get; set; } = new List<PreguntaVersion>();
}

