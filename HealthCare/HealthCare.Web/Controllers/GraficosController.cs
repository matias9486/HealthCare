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
    public class GraficosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager; 
        
        public GraficosController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
                
        public IActionResult GraficoTratamientos()
        {                                    
            return View();
        }

        public IActionResult GraficoProductos()
        {
            return View();
        }

        public IActionResult GraficoTipoPatologias()
        {
            return View();
        }

        public IActionResult GraficoPatologias()
        {
            return View();
        }
        public JsonResult Productos(DateTime inicial, DateTime final,string filtrar)
        {
            
            var userId = _userManager.GetUserId(HttpContext.User);

            List<SeriePastel> lista= (from s in _context.Sesiones.Include(p => p.Producto).Where(p => p.UsuarioCreacion.Id == userId).Where(f => f.Fecha >= inicial && f.Fecha <= final).ToList()
                         group s by s.Producto.Nombre into grupo
                         select new SeriePastel()
                         {
                             name = grupo.Key,
                             y = grupo.Count(),
                         }).ToList();
            

            GraficoPastel grafico = new GraficoPastel(filtrar,lista);            
            return Json(grafico);
        }


        public JsonResult Tratamientos(DateTime inicial, DateTime final, string filtrar)
        {

            var userId = _userManager.GetUserId(HttpContext.User);

            List<SeriePastel> lista = (from s in _context.Sesiones.Include(t => t.Tratamiento).Where(p => p.UsuarioCreacion.Id == userId).Where(f => f.Fecha >= inicial && f.Fecha <= final).ToList()
                                       group s by s.Tratamiento.Nombre into grupo
                                       select new SeriePastel()
                                       {
                                           name = grupo.Key,
                                           y = grupo.Count(),
                                       }).ToList();


            GraficoPastel grafico = new GraficoPastel(filtrar, lista);
            return Json(grafico);
        }

        public JsonResult TipoPatologias(DateTime inicial, DateTime final, string filtrar)
        {

            var userId = _userManager.GetUserId(HttpContext.User);

            List<SeriePastel> lista = (from s in _context.Sesiones.Include(t => t.Patologia.Tipo).Where(p => p.UsuarioCreacion.Id == userId).Where(f => f.Fecha >= inicial && f.Fecha <= final).ToList()
                                       group s by s.Patologia.Tipo.Nombre into grupo
                                       select new SeriePastel()
                                       {
                                           name = grupo.Key,
                                           y = grupo.Count(),
                                       }).ToList();


            GraficoPastel grafico = new GraficoPastel("Tipo de Patología", lista);
            return Json(grafico);
        }

        public JsonResult Patologias(DateTime inicial, DateTime final, string filtrar)
        {

            var userId = _userManager.GetUserId(HttpContext.User);

            List<SeriePastel> lista = (from s in _context.Sesiones.Include(t => t.Patologia).Where(p => p.UsuarioCreacion.Id == userId).Where(f => f.Fecha >= inicial && f.Fecha <= final).ToList()
                                       group s by s.Patologia.Nombre into grupo
                                       select new SeriePastel()
                                       {
                                           name = grupo.Key,
                                           y = grupo.Count(),
                                       }).ToList();


            GraficoPastel grafico = new GraficoPastel(filtrar, lista);
            return Json(grafico);
        }

        
    }
}
