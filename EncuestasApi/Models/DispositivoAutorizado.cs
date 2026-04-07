using System;
using System.Collections.Generic;

namespace EncuestasApi.Models;

public class DispositivoAutorizado
{
    public int IdDispositivo { get; set; }
    public int? IdUsuario { get; set; }
    public string? Identificador { get; set; }
    public string? SerialDispositivo { get; set; }
    public bool? Activo { get; set; }
    public DateOnly? FechaCreacion { get; set; }
    public DateOnly? FechaModificacion { get; set; }

    public Usuario? Usuario { get; set; }
    public ICollection<Encuesta> Encuestas { get; set; } = new List<Encuesta>();
}
