using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models
{
    public class ModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public string Nombre { get; set; }
        
        public bool Activo { get; set; }

        public string UsuarioCreacionId { get; set; }
        public Usuario UsuarioCreacion { get; set; }
    }
}
