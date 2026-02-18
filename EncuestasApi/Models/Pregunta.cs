using System;
using System.Collections.Generic;

namespace EncuestasApi.Models;

public partial class Pregunta
{
    public int Id { get; set; }

    public int EncuestaId { get; set; }

    public int? SeccionId { get; set; }

    public string Texto { get; set; } = null!;

    public int? TipoControlId { get; set; }

    public int? Orden { get; set; }

    public bool Requerido { get; set; }

    public int EstatusId { get; set; }

    public int UsuarioId { get; set; }

    public bool Agregar { get; set; }

    public bool ConOpciones { get; set; }

    public string? Placeholder { get; set; }

    public virtual Encuesta Encuesta { get; set; } = null!;

    public virtual ICollection<Opcione> Opciones { get; set; } = new List<Opcione>();

    public virtual ICollection<Respuesta> Respuesta { get; set; } = new List<Respuesta>();

    public virtual Seccione? Seccion { get; set; }

    public virtual TiposControle? TipoControl { get; set; }

    public virtual Estatus Usuario { get; set; } = null!;

    public virtual Usuario UsuarioNavigation { get; set; } = null!;
}
