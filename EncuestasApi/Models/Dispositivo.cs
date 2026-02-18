using System;
using System.Collections.Generic;

namespace EncuestasApi.Models;

public partial class Dispositivo
{
    public int Id { get; set; }

    public string DeviceId { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<EncuestasRespondida> EncuestasRespondida { get; set; } = new List<EncuestasRespondida>();
}
