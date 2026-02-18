using System;
using System.Collections.Generic;

namespace EncuestasApi.Models;

public partial class Subopcione
{
    public int Id { get; set; }

    public int OpcionId { get; set; }

    public string Texto { get; set; } = null!;

    public int TipoControlId { get; set; }

    public bool ConObservaciones { get; set; }

    public bool ConFecha { get; set; }

    public virtual Opcione Opcion { get; set; } = null!;

    public virtual ICollection<Respuesta> Respuesta { get; set; } = new List<Respuesta>();

    public virtual TiposControle TipoControl { get; set; } = null!;
}
