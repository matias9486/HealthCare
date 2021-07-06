using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models
{
    public class TipoPatologia:ModelBase
    {
        /*
         https://docs.microsoft.com/es-mx/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key 
         */
        //relacion 1-N. Relacion totalmente definida. 

        /*
         Propiedad de navegación: Propiedad definida en la entidad principal o dependiente que hace referencia a la entidad relacionada.
         definidas propiedades de navegación en ambos extremos de la relación y una propiedad de clave externa definida en la clase de entidad dependiente.
        -Si se encuentra un par de propiedades de navegación entre dos tipos, se configurarán como propiedades de navegación inversa de la misma relación.
        -Si la entidad dependiente contiene una propiedad con un nombre que coincide con uno de estos patrones, se configurará como clave externa:
        */
        public List<Patologia> Patologias { get; set; }
    }
}
