using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Models.Repositorios
{
    public class RepositorioProductos
    {
        private static RepositorioProductos instance = null;
        private List<Producto> lista = new List<Producto>();

        public static RepositorioProductos instanciar()
        {
            if (instance == null)
            {
                instance = new RepositorioProductos();
            }
            return instance;
        }

        public void agregar(Producto nuevo)
        {
            lista.Add(nuevo);
        }

        public List<Producto> devolverLista()
        {
            return lista;
        }

    }
}
