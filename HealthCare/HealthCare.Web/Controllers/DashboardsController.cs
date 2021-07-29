using HealthCare.Web.Data;
using HealthCare.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace HealthCare.Web.Controllers
{
    [Authorize]
    public class DashboardsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager; //servirá para guardar mi usuario

        //se agrego atributo userManager al constructor para tener el usuario que esta en sesion
        public DashboardsController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;// uso el atributo y parametro que agregue para tener el usuario logueado
        }
        public IActionResult Index()
        {

            return View();
        }


        public JsonResult DataPastel()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            //filtro los tratamientos por el id del usuario y tratamiento activo
            //var lista =  _context.Tratamientos.Where(p => p.UsuarioCreacion.Id == userId && p.Activo == true).ToListAsync();

            /*
            var tratamientosPorNombre =
                from t in lista.Result
                group t by t.Nombre into grupoTratamientos
                select new
                {
                    nombre = grupoTratamientos.Key,
                    cantidad = grupoTratamientos.Count(),
                };

            List<SeriePastel> listaTratamientos = new List<SeriePastel>();
            foreach (var item in tratamientosPorNombre)
            {
                SeriePastel serie = new SeriePastel();
                serie.name = item.nombre;
                serie.y = item.cantidad;
                listaTratamientos.Add(serie);
            }
            */

            /*
            List<SeriePastel> listaTratamientos= (from t in _context.Tratamientos.Where(p => p.UsuarioCreacion.Id == userId && p.Activo == true).ToList()
            group t by t.Nombre into grupoTratamientos
                                                 select new SeriePastel()
                                                 {
                                                     name = grupoTratamientos.Key,
                                                     y = grupoTratamientos.Count(),
                                                 }).ToList();
            
            List<SeriePastel> lista = (from t in _context.Sesiones.Include(t=>t.Tratamiento).Where(p => p.UsuarioCreacion.Id == userId).ToList()
                                                   group t by t.Tratamiento.Nombre into grupo
                                                   select new SeriePastel()
                                                   {
                                                       name = grupo.Key,
                                                       y = grupo.Count(),
                                                   }).ToList();
            */
            List<SeriePastel> lista = (from s in _context.Sesiones.Include(p => p.Producto).Where(p => p.UsuarioCreacion.Id == userId).ToList()
                                       group s by s.Producto.Nombre into grupo
                                       select new SeriePastel()
                                       {
                                           name = grupo.Key,
                                           y = grupo.Count(),
                                       }).ToList();

            //SeriePastel serie = new SeriePastel();
            //return Json(serie.GetDataDummy());
            return Json(lista);
        }


        //probando
        public IActionResult Grafico(DateTime fechaInicial,DateTime fechaFinal)
        {                        
            TempData["mensaje"] = "Inicial: "+ fechaInicial.ToString("yyyy/MM/dd HH:mm") + ". Final: "+ fechaFinal;
            TempData["tipo"] = "alert-primary";


            
            ViewBag.Inicial = fechaInicial;
            ViewBag.Final = fechaFinal;
            RedirectToAction( "graficaFiltrada");

            return View();
        }

        public JsonResult graficaFiltrada(DateTime inicial, DateTime final)
        {
            //DateTime inicial = ViewBag.Inicial;
            //DateTime final = ViewBag.Final;
            var userId = _userManager.GetUserId(HttpContext.User);
            
            List<SeriePastel> lista = (from s in _context.Sesiones.Include(p => p.Producto).Where(p => p.UsuarioCreacion.Id == userId && p.Fecha>=inicial && p.Fecha<=final).ToList()
                                       group s by s.Producto.Nombre into grupo
                                       select new SeriePastel()
                                       {
                                           name = grupo.Key,
                                           y = grupo.Count(),
                                       }).ToList();

            
            return Json(lista);
        }

        


    }
}
