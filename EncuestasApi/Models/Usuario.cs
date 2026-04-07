using System;
using System.Collections.Generic;

namespace EncuestasApi.Models;

public class Usuario
{
    public int IdUsuario { get; set; }
    public string? NombreUsuario { get; set; }
    public string? Password { get; set; }
    public string? Brigada { get; set; }
    public string? Jurisdiccion { get; set; }
    public string? Rol { get; set; }
    public bool? Activo { get; set; }
    public DateOnly? FechaCreacion { get; set; }
    public DateOnly? FechaModificacion { get; set; }

    public ICollection<DispositivoAutorizado> DispositivosAutorizados { get; set; } = new List<DispositivoAutorizado>();
    public ICollection<Encuesta> Encuestas { get; set; } = new List<Encuesta>();
}