using System;
using System.Collections.Generic;

namespace EncuestasApi.Models;

public class PreguntaVersion
{
    public int IdPregunta { get; set; }
    public int? IdVersion { get; set; }
    public string? Pregunta { get; set; }
    public int? Especificacion { get; set; }
    public string? Seccion { get; set; }
    public bool? Obligatoria { get; set; }

    public VersionEncuesta? Version { get; set; }
    public PreguntaVersion? PreguntaPadre { get; set; }
    public ICollection<PreguntaVersion> SubPreguntas { get; set; } = new List<PreguntaVersion>();
}