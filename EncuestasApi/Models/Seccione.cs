using System;
using System.Collections.Generic;

namespace EncuestasApi.Models;

public partial class Seccione
{
    public int Id { get; set; }

    public int EncuestaId { get; set; }

    public string Titulo { get; set; } = null!;

    public virtual Encuesta Encuesta { get; set; } = null!;

    public virtual ICollection<Pregunta> Pregunta { get; set; } = new List<Pregunta>();
}
