using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EncuestasApp.utilerias
{
    public static class GeneraCurp
    {
        private static readonly string[] Estados =
        {
            "AS","BC","BS","CC","CL","CM","CS","CH","DF","DG","GT",
            "GR","HG","JC","MC","MN","MS","NT","NL","OC","PL","QT",
            "QR","SP","SL","SR","TC","TS","TL","VZ","YN","ZS",
            "NE","EX"
        };

        private static readonly string[] Altisonantes =
{
"BACA","BAKA","BUEI","BUEY","CACA","CACO","CAGA","CAGO","CAKA","CAKO",
"COGE","COGI","COJA","COJE","COJI","COJO","COLA","CULO",
"FALO","FETO","GETA","GUEI","GUEY","JETA","JOTO",
"KACA","KACO","KAGA","KAGO","KAKA","KAKO","KOGE","KOGI",
"KOJA","KOJE","KOJI","KOJO","KOLA","KULO",
"LILO","LOCA","LOCO","LOKA","LOKO",
"MAME","MAMO","MEAR","MEAS","MEON","MIAR","MION","MOCO","MOKO","MULA","MULO",
"NACA","NACO","PEDA","PEDO","PENE","PIPI","PITO","POPO","PUTA","PUTO",
"QULO","RATA","ROBA","ROBE","ROBO","RUIN","SENO","TETA",
"VACA","VAGA","VAGO","VAKA","VUEI","VUEY","WUEI","WUEY"
};

        private static readonly string[] Preposiciones =
{
"DA","DAS","DE","DEL","DER","DI","DIE",
"DD","Y","EL","LA","LOS","LAS","LE","LES",
"MAC","MC","VAN","VON"
};

        private static readonly string[] NoSonNombres =
        {
"J","J.","JOSE","JOSÉ","M","M.","MA","MA.","MARIA","MARÍA"
};

        public static string GenerarCurp(
     string nombre,
     string apellido1,
     string apellido2,
     DateTime fechaNacimiento,
     string genero,
     string entidadCodigo)
        {
            nombre = Limpiar(nombre);
            apellido1 = Limpiar(apellido1);
            apellido2 = Limpiar(apellido2);

            string porciones = GenerarPorciones(apellido1, apellido2, nombre);

            string porcion1 = porciones.Substring(0, 4);
            string porcion2 = porciones.Substring(4);

            string fecha = fechaNacimiento.ToString("yyMMdd");
            string siglo = fechaNacimiento.Year < 2000 ? "0" : "A";
            string sexo = genero == "Femenino" ? "M" : "H";

            string curp17 = (porcion1 + fecha + sexo + entidadCodigo + porcion2 + siglo).ToUpper();

            int digito = CalcularDigito(curp17);

            return curp17 + digito;
        }

        private static string GenerarPorciones(string apellido1, string apellido2, string nombre)
        {
            string palabra;

            // Apellido 1
            palabra = EncuentraPalabra(apellido1);

            string porcion1 = Sustituir(palabra[0]) +
                              PrimeraVocalInterna(palabra) +
                              (apellido2.Length > 0 ? Sustituir(EncuentraPalabra(apellido2)[0]) : "X") +
                              Sustituir(PrimeraLetraNombre(nombre)[0]);

            string porcion2 =
                PrimeraConsonanteInterna(palabra) +
                (apellido2.Length > 0 ? PrimeraConsonanteInterna(EncuentraPalabra(apellido2)) : "X") +
                PrimeraConsonanteInterna(EncuentraNombreReal(nombre));

            if (Altisonantes.Contains(porcion1))
                porcion1 = porcion1[0] + "X" + porcion1.Substring(2);

            return porcion1 + porcion2;
        }

        private static string Limpiar(string texto)
        {
            texto = texto.ToUpper().Trim();
            texto = Regex.Replace(texto, @"[^A-ZÑ\s]", "");
            return texto;
        }

        private static string ObtenerPrimerLetraYVocal(string texto)
        {
            char primera = texto[0];
            char vocal = texto.Skip(1).FirstOrDefault(c => "AEIOU".Contains(c));
            if (vocal == default) vocal = 'X';
            return $"{primera}{vocal}";
        }

        private static string ObtenerNombreReal(string nombre)
        {
            string[] ignorar = { "J", "J.", "JOSE", "JOSÉ", "M", "M.", "MA", "MA.", "MARIA", "MARÍA" };
            var partes = nombre.Split(' ');
            string real = partes.FirstOrDefault(n => !ignorar.Contains(n)) ?? partes[0];
            return real[0].ToString();
        }

        private static string ObtenerConsonanteInterna(string texto)
        {
            var consonante = texto.Skip(1)
                .FirstOrDefault(c => !"AEIOU".Contains(c));
            return consonante == default ? "X" : consonante.ToString();
        }

        private static int CalcularDigito(string curp)
        {
            string diccionario = "0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
            int suma = 0;

            for (int i = 0; i < 17; i++)
            {
                int valor = diccionario.IndexOf(curp[i]);
                suma += valor * (18 - i);
            }

            int residuo = suma % 10;
            return residuo == 0 ? 0 : 10 - residuo;
        }

        public static bool CoincideConDatos(string curp,string nombre,string apellido1,string apellido2,DateTime fechaNacimiento,string genero,string entidadCodigo)
        {
            if (string.IsNullOrWhiteSpace(curp) || curp.Length != 18)
                return false;

            curp = curp.ToUpper();

            string curpGenerada = GenerarCurp(
                nombre,
                apellido1,
                apellido2,
                fechaNacimiento,
                genero,
                entidadCodigo);

            // Comparar solo hasta posición 16
            return curp.Substring(0, 16) ==
                   curpGenerada.Substring(0, 16);
        }

        private static string EncuentraPalabra(string texto)
        {
            var palabras = texto.Split(' ');
            foreach (var p in palabras)
            {
                if (!Preposiciones.Contains(p))
                    return p;
            }
            return palabras[0];
        }

        private static string EncuentraNombreReal(string nombre)
        {
            var partes = nombre.Split(' ');
            foreach (var p in partes)
            {
                if (!Preposiciones.Contains(p) && !NoSonNombres.Contains(p))
                    return p;
            }
            return partes[0];
        }

        private static string PrimeraVocalInterna(string texto)
        {
            foreach (var c in texto.Skip(1))
                if ("AEIOUÁÉÍÓÚÜ".Contains(c))
                    return Sustituir(c);
            return "X";
        }

        private static string PrimeraConsonanteInterna(string texto)
        {
            foreach (var c in texto.Skip(1))
                if ("BCDFGHJKLMNPQRSTVWXYZÑ".Contains(c))
                    return Sustituir(c);
            return "X";
        }

        private static string PrimeraLetraNombre(string nombre)
        {
            return EncuentraNombreReal(nombre)[0].ToString();
        }

        private static string Sustituir(char c)
        {
            return c switch
            {
                'Á' => "A",
                'É' => "E",
                'Í' => "I",
                'Ó' => "O",
                'Ú' => "U",
                'Ü' => "U",
                'Ñ' => "X",
                _ => c.ToString()
            };
        }


    }




}
