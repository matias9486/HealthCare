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
        private readonly UserManager<Usuario> _userManager; 
        
        public DashboardsController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {

            return View();
        }


        public JsonResult DataPastel()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            
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

            
            return Json(lista);
        }


        
        public IActionResult Grafico()
        {                                    
            return View();
        }

        public JsonResult graficaFiltrada(DateTime inicial, DateTime final)
        {
            
            var userId = _userManager.GetUserId(HttpContext.User);
            
            List<SeriePastel> lista = (from s in _context.Sesiones.Include(p => p.Producto).Where(p => p.UsuarioCreacion.Id == userId).Where(f=> f.Fecha>=inicial && f.Fecha<=final).ToList()
                                       group s by s.Producto.Nombre into grupo
                                       select new SeriePastel()
                                       {
                                           name = grupo.Key,
                                           y = grupo.Count(),
                                       }).ToList();

            GraficoPastel grafico = new GraficoPastel("Productos",lista);
            //return Json(lista);
            return Json(grafico);
        }
        
    }
}
