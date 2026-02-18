using System;
using System.Collections.Generic;

namespace EncuestasApi.Models;

public partial class Estatus
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Pregunta> Pregunta { get; set; } = new List<Pregunta>();
}
