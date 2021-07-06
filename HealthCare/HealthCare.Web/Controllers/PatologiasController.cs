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

namespace HealthCare.Web.Controllers
{
    public class PatologiasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public PatologiasController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Patologias
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var lista = await _context.Patologias.Include(t=>t.Tipo).Include(u => u.UsuarioCreacion).Where(p => p.UsuarioCreacionId == userId && p.Activo == true).ToListAsync();            
            return View(lista);
        }

        // GET: Patologias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patologia = await _context.Patologias
                .Include(p => p.Tipo)
                .Include(p => p.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patologia == null)
            {
                return NotFound();
            }

            return View(patologia);
        }

        // GET: Patologias/Create
        public IActionResult Create()
        {
            //filtre la lista de tipos para que figuren los activos
            ViewData["TipoId"] = new SelectList(_context.TipoPatologias.Where(t=>t.Activo==true), "Id", "Nombre");
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }

        // POST: Patologias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("TipoId,Id,Nombre,Activo,UsuarioCreacionId")] Patologia patologia)
        public async Task<IActionResult> Create([Bind("TipoId,Id,Nombre")] Patologia patologia)
        {
            if (ModelState.IsValid)
            {
                patologia.UsuarioCreacionId = _userManager.GetUserId(HttpContext.User);
                patologia.Activo = true;

                _context.Add(patologia);
                await _context.SaveChangesAsync();

                //agregado
                TempData["mensaje"] = "Se agregó Patología con éxito.";
                TempData["tipo"] = "alert-success";
                //-------------------------------------       
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoId"] = new SelectList(_context.TipoPatologias, "Id", "Nombre", patologia.TipoId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", patologia.UsuarioCreacionId);
            return View(patologia);
        }

        // GET: Patologias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patologia = await _context.Patologias.FindAsync(id);
            if (patologia == null)
            {
                return NotFound();
            }
            ViewData["TipoId"] = new SelectList(_context.TipoPatologias, "Id", "Nombre", patologia.TipoId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", patologia.UsuarioCreacionId);
            return View(patologia);
        }

        // POST: Patologias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("TipoId,Id,Nombre,Activo,UsuarioCreacionId")] Patologia patologia)
        public async Task<IActionResult> Edit(int id, [Bind("TipoId,Id,Nombre")] Patologia patologia)
        {
            if (id != patologia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //agregado
                    patologia.UsuarioCreacionId = _userManager.GetUserId(HttpContext.User);
                    patologia.Activo = true;

                    _context.Update(patologia);
                    await _context.SaveChangesAsync();

                    //agregado
                    TempData["mensaje"] = "Se modificó Patología con éxito.";
                    TempData["tipo"] = "alert-success";
                    //----------------------------------------                    

                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatologiaExists(patologia.Id))
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
            ViewData["TipoId"] = new SelectList(_context.TipoPatologias, "Id", "Nombre", patologia.TipoId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", patologia.UsuarioCreacionId);
            return View(patologia);
        }

        // GET: Patologias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patologia = await _context.Patologias
                .Include(p => p.Tipo)
                .Include(p => p.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patologia == null)
            {
                return NotFound();
            }

            return View(patologia);
        }

        // POST: Patologias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patologia = await _context.Patologias.FindAsync(id);

            //agregado para poner como inactivo el registro sin eliminarlo. Baja lógica
            patologia.Activo = false;
            _context.Update(patologia);

            //_context.Patologias.Remove(patologia);
            await _context.SaveChangesAsync();

            //agregado
            TempData["mensaje"] = "Se eliminó Patología con éxito.";
            TempData["tipo"] = "alert-success";
                        
            return RedirectToAction(nameof(Index));
        }

        private bool PatologiaExists(int id)
        {
            return _context.Patologias.Any(e => e.Id == id);
        }
    }
}
