using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models
{
    public class Tratamiento
    {
        public static int ID_Incremental = 0;
        public int Id { get; set; }
        public int Nombre { get; set; }
        public int Precio { get; set; }

        public Tratamiento(int nombre, int precio)
        {
            ID_Incremental++;
            Id = ID_Incremental;
            Nombre = nombre;
            Precio = precio;
        }

        public Tratamiento(int id, int nombre, int precio)
        {
            Id = id;
            Nombre = nombre;
            Precio = precio;
        }

        public override string ToString()
        {
            return $"{Nombre}, ${Precio}";
        }
    }
}
