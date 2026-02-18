using System;
using System.Collections.Generic;

namespace EncuestasApi.Models;

public partial class Opcione
{
    public int Id { get; set; }

    public int PreguntaId { get; set; }

    public string? Texto { get; set; }

    public string? Placeholder { get; set; }

    public bool ConSubopciones { get; set; }

    public int? TipoControlId { get; set; }

    public virtual Pregunta Pregunta { get; set; } = null!;

    public virtual ICollection<Subopcione> Subopciones { get; set; } = new List<Subopcione>();

    public virtual TiposControle? TipoControl { get; set; }
}
