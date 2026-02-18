using System;
using System.Collections.Generic;

namespace EncuestasProxyApi.Models;

public partial class Dispositivo
{
    public int Id { get; set; }

    public string DeviceId { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }
}
