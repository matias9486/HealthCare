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
    public class TipoPatologiasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipoPatologiasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TipoPatologias
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TipoPatologias.Include(t => t.UsuarioCreacion);
            return View(await applicationDbContext.ToListAsync());
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
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id");
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
                _context.Add(tipoPatologia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", tipoPatologia.UsuarioCreacionId);
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
                    _context.Update(tipoPatologia);
                    await _context.SaveChangesAsync();
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
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", tipoPatologia.UsuarioCreacionId);
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
            _context.TipoPatologias.Remove(tipoPatologia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoPatologiaExists(int id)
        {
            return _context.TipoPatologias.Any(e => e.Id == id);
        }
    }
}
