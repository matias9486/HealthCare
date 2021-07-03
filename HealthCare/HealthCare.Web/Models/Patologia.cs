using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models
{
    public class Patologia:ModelBase
    {
        [Display(Name = "Tipo")]

        public int TipoId { get; set; }
        public TipoPatologia Tipo { get; set; }
    }
}
