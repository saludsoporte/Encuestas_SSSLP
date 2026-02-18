using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncuestasApp.Models
{
    public class EncuestaTest
    {
        public string NombreCompleto { get; set; } = "IVAN DOMINGUEZ";
        public int Edad { get; set; } = 42;
        public string Genero { get; set; } = "MASCULINO";
        public DateTime FechaNacimiento { get; set; } = DateTime.Now;
        public string Curp { get; set; } = "DOTH831005HSPMRS04";
        public string SeccionElectoral { get; set; } = "0069";
        public string Calle { get; set; } = "CARMEN SERDAN";
        public string NumExterior { get; set; } = "170";
        public string? NumInterior { get; set; } = "A";
        public string Colonia { get; set; } = "LA VICTORIA";
        public string Localidad { get; set; } = "SAN LUIS POTOSI";
        public string Municipio { get; set; } = "SAN LUIS POTOSI";
        public string CodigoPostal { get; set; } = "78230";
        public string Telefono { get; set; } = "4448149436";
        public string Celular { get; set; } = "4441234567";
        public string EstadoCivil { get; set; } = "SOLTERO";
        public string Ocupacion { get; set; } = "PROGRAMADOR";
    }
}
