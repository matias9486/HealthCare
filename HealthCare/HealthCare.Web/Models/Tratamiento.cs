using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models
{
    public class Tratamiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public string Nombre { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "{0} es requerido.")]
        //[Range(1, 1000, ErrorMessage =" {0} debe estar entre {1} y {1000}.")]
        
        public double Precio { get; set; }

        public bool Activo { get; set; }

        public string UsuarioCreacionId { get; set; }
        public Usuario UsuarioCreacion { get; set; }

        public Tratamiento(){}
        public Tratamiento(string nombre, double precio)
        {
            Nombre = nombre;
            Precio = precio;
            Activo = true;
        }

        public Tratamiento(int id, string nombre, double precio, bool activo)
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
