using System;
using System.Collections.Generic;

namespace EncuestasApi.Models;

public partial class UsuariosRole
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public int? RolId { get; set; }

    public virtual Role? Rol { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
