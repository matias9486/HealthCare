using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models
{
    public class Mensaje
    {
        public string Texto { get; set; }
        public string Tipo { get; set; }

        public Mensaje(string texto, string tipo)
        {
            Texto = texto;
            Tipo = tipo;
        }
    }
}
