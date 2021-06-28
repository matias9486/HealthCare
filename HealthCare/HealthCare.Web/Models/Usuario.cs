using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models
{
    public class Usuario:IdentityUser//<int>  //se agregó generic <int> para id entero
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
