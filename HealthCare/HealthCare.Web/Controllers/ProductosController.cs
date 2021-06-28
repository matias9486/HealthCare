using HealthCare.Web.Data;
using HealthCare.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext context;

        public ProductosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult AgregarProducto()
        {            
            return View();
        }

        public IActionResult GuardarProducto(string nombre, string precio)
        {

            Producto nuevo = new Producto(nombre, double.Parse(precio));
            Mensaje mensaje;
            
            Producto guardar=context.Productos.FirstOrDefault(i => i.Nombre.ToUpper() == nuevo.Nombre.ToUpper() && i.Precio== nuevo.Precio);
            
           if(guardar==null)
            {                
                context.Add(nuevo);
                context.SaveChanges();
                mensaje = new Mensaje("Se agregó Producto con éxito", "alert-success");
            }
            else 
            {
                mensaje = new Mensaje("Ya existe producto con ese nombre y precio", "alert-danger");
            }
           
                        
            //uso tempdata para pasar datos entre acciones. Podria uar variables de sesion o es demasiado para mostrar un msj??
            //TempData almacena la informacion hasta que es accedida. permite pasar datos entre acciones 
            TempData["mensaje"] = mensaje.Texto;
            TempData["tipo"] = mensaje.Tipo;

            //return RedirectToAction("MostrarMensaje", new {ms=mensaje.Texto, tipo=mensaje.Tipo });  //A-otra forma de pasar los valores
            return RedirectToAction("MostrarMensaje");
        }



        //public IActionResult MostrarMensaje(string ms,string tipo) //A- otra forma de recibir valores
        public IActionResult MostrarMensaje()
        {
            //levanto los tempData que defini en la accion guardar
            string texto, tipo;
            //controlo que exista algun tempdata para mostra mensaje y evitar que al actualizr la pagina salga error
            if (TempData.Values.Count==0)
            {
                texto = "Elija una acción a realizar desde el menu";
                tipo = "alert-danger";
            }
            else
            {
                /*
                //accedo al tempData y se elimina una vez accedido. Si actualiza da error porque ya se elimino el dato
                texto = TempData["mensaje"].ToString();
                tipo = TempData["tipo"].ToString();
                */
                
                //peek permite mantener el tempData luego de ser accedido. No lo borra. No da error al actualizar porque no se borro el dato
                texto = TempData.Peek("mensaje").ToString();
                tipo = TempData.Peek("tipo").ToString();
            }
            
            Mensaje mensaje = new Mensaje(texto,tipo);
            return View("Mensaje",mensaje);
        }
        public IActionResult MostrarProductos()
        {
            
            return View(context.Productos.ToList());
        }

        public IActionResult EditarProducto(int id)
        {
            Producto editar = context.Productos.FirstOrDefault(i => i.Id == id);

            return View(editar);
        }

        public IActionResult Actualizar(int id, string nombre, double precio, bool activo)
        {
            Mensaje mensaje;
            Producto editar = context.Productos.FirstOrDefault(i => i.Id == id);
            editar.Nombre = nombre;
            editar.Precio = precio;
            editar.Activo = activo;

            Producto buscado = context.Productos.FirstOrDefault(i => i.Nombre.ToUpper() == editar.Nombre.ToUpper() && i.Precio == editar.Precio);

            if (buscado == null)
            {
                context.Productos.Update(editar);
                context.SaveChanges();
                mensaje = new Mensaje("Se modificó Producto con éxito", "alert-success");
            }
            else
            {
                mensaje = new Mensaje("Ya existe producto con ese nombre y precio", "alert-danger");
            }
            

            //return RedirectToAction("MostrarProductos",context.Productos.ToList());
            TempData["mensaje"] = mensaje.Texto;
            TempData["tipo"] = mensaje.Tipo;

            return RedirectToAction("MostrarMensaje");
        }

        public IActionResult EliminarProducto(int id)
        {
            Producto eliminar = context.Productos.FirstOrDefault(i => i.Id == id);
            eliminar.Activo= false;
            context.Productos.Update(eliminar);
            context.SaveChanges();
            TempData["mensaje"] = "Se eliminó Producto con éxito";
            TempData["tipo"] = "alert-success";

            return RedirectToAction("MostrarMensaje");
        }
        public ActionResult Index()
        {
            return View();
        }

        
    }
}
