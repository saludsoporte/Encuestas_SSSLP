using System;
using System.Collections.Generic;

namespace EncuestasApi.Models;

public partial class Respuesta
{
    public int Id { get; set; }

    public int EncuestaRespondidaId { get; set; }

    public int? PreguntaId { get; set; }

    public int? OpcionId { get; set; }

    public int? SubopcionId { get; set; }

    public string DetalleRespuesta { get; set; } = null!;

    public virtual EncuestasRespondida EncuestaRespondida { get; set; } = null!;

    public virtual Pregunta? Pregunta { get; set; }

    public virtual Subopcione? Subopcion { get; set; }
}
