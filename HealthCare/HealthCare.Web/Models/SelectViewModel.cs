using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models
{
    public class SelectViewModel
    {            
        //clase utilizada para poder crear una lista auxiliar en la cual poner en Value el texto que quiera(varios atributos de una clase) y pasarlo luego al combobox
        //de otra forma solo podria poner un solo atributo para mostrar en el combobox
        public int ID { get; set; }            
        public string Value { get; set; }        
    }
}
