using System;
using System.Collections.Generic;

namespace EncuestasApi.Models;

public partial class Encuesta
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaCierre { get; set; }

    public int UsuarioId { get; set; }

    public DateTime UltimaActualizacion { get; set; }

    public virtual ICollection<EncuestasRespondida> EncuestasRespondida { get; set; } = new List<EncuestasRespondida>();

    public virtual ICollection<Pregunta> Pregunta { get; set; } = new List<Pregunta>();

    public virtual ICollection<Seccione> Secciones { get; set; } = new List<Seccione>();

    public virtual Usuario Usuario { get; set; } = null!;
}
