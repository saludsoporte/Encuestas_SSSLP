using System;
using System.Collections.Generic;

namespace EncuestasApi.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? PasswordHash { get; set; }

    public string? FullName { get; set; }

    public virtual ICollection<Encuesta> Encuesta { get; set; } = new List<Encuesta>();

    public virtual ICollection<EncuestasRespondida> EncuestasRespondida { get; set; } = new List<EncuestasRespondida>();

    public virtual ICollection<Pregunta> Pregunta { get; set; } = new List<Pregunta>();

    public virtual ICollection<UsuariosRole> UsuariosRoles { get; set; } = new List<UsuariosRole>();
}
