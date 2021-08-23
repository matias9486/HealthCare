using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models
{
    public class GraficoPastel
    {
        public string Nombre { get; set; }

        public List<SeriePastel> lista { get; set; }

        public GraficoPastel()
        {

        }

        public GraficoPastel(string nombre, List<SeriePastel> serie)
        {
            Nombre = nombre;
            lista = serie;
        }


    }
}
