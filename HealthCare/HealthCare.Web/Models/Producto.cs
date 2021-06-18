using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }

        public bool Activo { get; set; }
        public Producto(string nombre, double precio)
        {         
            Nombre = nombre;
            Precio = precio;
            Activo = true;
        }

        public Producto(int id, string nombre, double precio, bool activo)
        {
            Id = id;
            Nombre = nombre;
            Precio = precio;
            Activo = activo;
        }

        public override string ToString()
        {
            return $"{Nombre}, ${Precio}";
        }
    }
}
