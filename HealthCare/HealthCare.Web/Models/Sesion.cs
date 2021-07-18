using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models
{
    public class Sesion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UsuarioCreacionId { get; set; }
        public Usuario UsuarioCreacion { get; set; }

        [Required(ErrorMessage = "{0} es requerido.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }


        [Display(Name = "Paciente")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; }

        /* comentando por error cicliclo patologia ya contiene tipo de patologia
        [Display(Name = "Tipo de Patología")]
        [Required(ErrorMessage = "{0} es requerido.")]
        
        
        public int TipoId { get; set; }
        public TipoPatologia Tipo { get; set; }
        */
        [Display(Name = "Patología")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public int PatologiaId { get; set; }
        public Patologia Patologia { get; set; }

        [Display(Name = "Tratamiento")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public int TratamientoId { get; set; }
        public Tratamiento Tratamiento { get; set; }

        [Display(Name = "Producto")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        [Display(Name = "Peso")]
        
        public double Peso { get; set; }

        [Display(Name = "Presión")]        
        public double Presion { get; set; }

        [Display(Name = "Observaciones")]        
        public string Observaciones { get; set; }
        [Display(Name = "Operaciones")]        
        public string Operaciones { get; set; }
        [Display(Name = "Medicación")]        
        public string Medicacion { get; set; }

        [Display(Name = "Automedicación")]       
        public string Automedicacion { get; set; }

        [Display(Name = "Diagnóstico")]        
        public string DiagnosticoMedico { get; set; }


        

    }
}
