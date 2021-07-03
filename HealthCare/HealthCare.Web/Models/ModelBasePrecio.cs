using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models
{
    public class ModelBasePrecio:ModelBase
    {
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "{0} es requerido.")]

        public double Precio { get; set; }
    }
}
