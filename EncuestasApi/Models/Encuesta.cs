using System;
using System.Collections.Generic;
using System.Text.Json;

namespace EncuestasApi.Models;

public class Encuesta
{
    public int IdEncuesta { get; set; }
    public int? IdUsuario { get; set; }
    public int? IdDispositivo { get; set; }
    public int? IdVersion { get; set; }
    public string? Curp { get; set; }
    public DateTime? FechaCaptura { get; set; }
    public DateTime? FechaSincronizacion { get; set; }
    public string? Latitud { get; set; }
    public string? Longitud { get; set; }
    public bool? PuertaAbierta { get; set; }
    public string? ObsPuerta { get; set; }
    public bool? Participa { get; set; }
    public string? ObsParticipa { get; set; }

    public JsonDocument? Seccion1 { get; set; }
    public JsonDocument? Seccion2 { get; set; }
    public JsonDocument? Seccion3 { get; set; }
    public JsonDocument? Seccion4 { get; set; }
    public JsonDocument? Seccion5 { get; set; }
    public JsonDocument? Seccion6 { get; set; }
    public JsonDocument? Seccion7 { get; set; }
    public JsonDocument? Seccion8 { get; set; }
    public JsonDocument? Seccion9 { get; set; }
    public JsonDocument? Seccion10 { get; set; }
    public JsonDocument? Seccion11 { get; set; }
    public JsonDocument? Seccion12 { get; set; }
    public JsonDocument? Seccion13 { get; set; }

    public Usuario? Usuario { get; set; }
    public DispositivoAutorizado? Dispositivo { get; set; }
    public VersionEncuesta? Version { get; set; }
}