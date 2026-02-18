using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;
namespace EncuestasApp.Models
{

    public class CodigoPostalCat
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public string Codigo { get; set; }

        public string Colonia { get; set; }
        public string Municipio { get; set; }
        public String cve_mun { get; set; }
        public string SeccionElectoral { get; set; }
    }
}



