using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthCare.Web.Data;
using HealthCare.Web.Models;

namespace HealthCare.Web.Controllers
{
    public class PatologiasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatologiasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Patologias
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Patologias.Include(p => p.Tipo).Include(p => p.UsuarioCreacion);
            return View(await applicationDbContext.ToListAsync());
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
            ViewData["TipoId"] = new SelectList(_context.TipoPatologias, "Id", "Nombre");
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }

        // POST: Patologias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipoId,Id,Nombre,Activo,UsuarioCreacionId")] Patologia patologia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patologia);
                await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Edit(int id, [Bind("TipoId,Id,Nombre,Activo,UsuarioCreacionId")] Patologia patologia)
        {
            if (id != patologia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patologia);
                    await _context.SaveChangesAsync();
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
            _context.Patologias.Remove(patologia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatologiaExists(int id)
        {
            return _context.Patologias.Any(e => e.Id == id);
        }
    }
}
