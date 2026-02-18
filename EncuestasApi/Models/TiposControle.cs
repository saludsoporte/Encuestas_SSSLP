using System;
using System.Collections.Generic;

namespace EncuestasApi.Models;

public partial class TiposControle
{
    public int Id { get; set; }

    public string Tipo { get; set; } = null!;

    public virtual ICollection<Opcione> Opciones { get; set; } = new List<Opcione>();

    public virtual ICollection<Pregunta> Pregunta { get; set; } = new List<Pregunta>();

    public virtual ICollection<Subopcione> Subopciones { get; set; } = new List<Subopcione>();
}
