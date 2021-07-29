using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthCare.Web.Data;
using HealthCare.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace HealthCare.Web.Controllers
{
    [Authorize]
    public class TipoPatologiasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        public TipoPatologiasController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TipoPatologias
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var lista = await _context.TipoPatologias.Where(p => p.UsuarioCreacion.Id == userId && p.Activo == true).Include(u => u.UsuarioCreacion).ToListAsync();
            return View(lista);

        }

        // GET: TipoPatologias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoPatologia = await _context.TipoPatologias
                .Include(t => t.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoPatologia == null)
            {
                return NotFound();
            }

            return View(tipoPatologia);
        }

        // GET: TipoPatologias/Create
        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (_context.TipoPatologias.Where(t => t.UsuarioCreacion.Id == userId).Count() == 0)
            {
                TempData["mensaje"] = "Tipo de Patologías es una clasificación para agrupar las distintas Patologías(Motivos del tratamiento) según el criterio del profesional.";
                TempData["tipo"] = "alert-primary";
            }
            return View();
        }

        // POST: TipoPatologias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Activo,UsuarioCreacionId")] TipoPatologia tipoPatologia)        
        {
            if (ModelState.IsValid)
            {
                tipoPatologia.UsuarioCreacionId = _userManager.GetUserId(HttpContext.User);
                tipoPatologia.Activo = true;

                _context.Add(tipoPatologia);
                await _context.SaveChangesAsync();

                //agregado
                TempData["mensaje"] = "Se agregó Tipo de Patología con éxito.";
                TempData["tipo"] = "alert-success";
                //-------------------------------------                
                return RedirectToAction(nameof(Index));
            }
            
            return View(tipoPatologia);
        }

        // GET: TipoPatologias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoPatologia = await _context.TipoPatologias.FindAsync(id);
            if (tipoPatologia == null)
            {
                return NotFound();
            }
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", tipoPatologia.UsuarioCreacionId);
            return View(tipoPatologia);
        }

        // POST: TipoPatologias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Activo,UsuarioCreacionId")] TipoPatologia tipoPatologia)        
        {
            if (id != tipoPatologia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //agregado
                    tipoPatologia.UsuarioCreacionId = _userManager.GetUserId(HttpContext.User);
                    tipoPatologia.Activo = true;

                    _context.Update(tipoPatologia);
                    await _context.SaveChangesAsync();

                    //agregado
                    TempData["mensaje"] = "Se modificó Tipo de Patología con éxito.";
                    TempData["tipo"] = "alert-success";
                    //----------------------------------------                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoPatologiaExists(tipoPatologia.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }            
            return View(tipoPatologia);
        }

        // GET: TipoPatologias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoPatologia = await _context.TipoPatologias
                .Include(t => t.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoPatologia == null)
            {
                return NotFound();
            }

            return View(tipoPatologia);
        }

        // POST: TipoPatologias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoPatologia = await _context.TipoPatologias.FindAsync(id);

            //agregado para poner como inactivo el registro sin eliminarlo. Baja lógica
            tipoPatologia.Activo = false;
            _context.Update(tipoPatologia);

            //_context.TipoPatologias.Remove(tipoPatologia);
            await _context.SaveChangesAsync();

            //agregado
            TempData["mensaje"] = "Se eliminó Tipo de Patología con éxito.";
            TempData["tipo"] = "alert-success";
                        
            return RedirectToAction(nameof(Index));
        }

        private bool TipoPatologiaExists(int id)
        {
            return _context.TipoPatologias.Any(e => e.Id == id);
        }
    }
}
