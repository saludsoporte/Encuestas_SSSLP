using System;
using System.Collections.Generic;

namespace EncuestasApi.Models;

public partial class EncuestasRespondida
{
    public int Id { get; set; }

    public int EncuestaId { get; set; }

    public string Curp { get; set; } = null!;

    public DateTime FechaInicio { get; set; }

    public DateTimeOffset? FechaFin { get; set; }

    public double? Latitud { get; set; }

    public double? Longitud { get; set; }

    public bool Terminada { get; set; }

    public bool Sincronizada { get; set; }

    public int? UsuarioId { get; set; }

    public DateTime? FechaSincronizacion { get; set; }

    public int? LocalId { get; set; }

    public int? DispositivoId { get; set; }

    public virtual Dispositivo? Dispositivo { get; set; }

    public virtual Encuesta Encuesta { get; set; } = null!;

    public virtual ICollection<Respuesta> Respuesta { get; set; } = new List<Respuesta>();

    public virtual Usuario? Usuario { get; set; }
}
