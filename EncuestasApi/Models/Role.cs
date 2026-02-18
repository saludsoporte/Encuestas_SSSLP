using System;
using System.Collections.Generic;

namespace EncuestasApi.Models;

public partial class Role
{
    public int Id { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<UsuariosRole> UsuariosRoles { get; set; } = new List<UsuariosRole>();
}
