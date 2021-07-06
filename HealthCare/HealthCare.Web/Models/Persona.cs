using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
namespace HealthCare.Web.Models
{
    public class Persona:ModelBase
    {
        [Display(Name = "Imagen")]
        public byte[] Imagen { get; set; }
                
        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public string Apellido { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "{0} es requerido.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        
    }
}
