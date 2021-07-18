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
    public class SesionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SesionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //mi accion para filtrar patologias por Tipo
        public List<Patologia> GetAll()
        {
            return _context.Patologias.ToList();
        }
        public List<Patologia> filtrarPatologias(int Tipo)
        {
            //var userId = _userManager.GetUserId(HttpContext.User);
            //var lista = _context.Patologias.Where(p => p.UsuarioCreacionId == userId && p.Activo == true).ToList();
            var lista = _context.Patologias.Where(p => p.TipoId==Tipo).ToList();
            return lista;
        }

        //Accion para filtrar combos por tipo de patologia... FUNCIONA
        public IActionResult filtrarPorTipo(int id)
        {
            var lista = _context.Patologias.Where(p => p.TipoId == id).ToList();//.Select(p => p.Diagnostico);

            return Json(lista);
        }


        // GET: Sesiones
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sesiones.Include(s => s.Paciente).Include(s => s.Patologia).Include(s => s.Producto).Include(s => s.Tratamiento).Include(s => s.UsuarioCreacion);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sesiones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sesion = await _context.Sesiones
                .Include(s => s.Paciente)
                .Include(s => s.Patologia)
                .Include(s => s.Producto)
                .Include(s => s.Tratamiento)
                .Include(s => s.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sesion == null)
            {
                return NotFound();
            }

            return View(sesion);
        }

        // GET: Sesiones/Create
        public IActionResult Create()
        {
            ViewData["PacienteId"] = new SelectList(_context.Paciente, "Id", "Apellido");
            ViewData["PatologiaId"] = new SelectList(_context.Patologias, "Id", "Nombre");
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre");
            ViewData["TratamientoId"] = new SelectList(_context.Tratamientos, "Id", "Nombre");
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id");

            ViewBag.TipoPatologias = new SelectList(_context.TipoPatologias, "Id", "Nombre");
            return View();
        }

        // POST: Sesiones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioCreacionId,Fecha,PacienteId,PatologiaId,TratamientoId,ProductoId,Peso,Presion,Observaciones,Operaciones,Medicacion,Automedicacion,DiagnosticoMedico")] Sesion sesion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sesion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PacienteId"] = new SelectList(_context.Paciente, "Id", "Apellido", sesion.PacienteId);
            ViewData["PatologiaId"] = new SelectList(_context.Patologias, "Id", "Nombre", sesion.PatologiaId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", sesion.ProductoId);
            ViewData["TratamientoId"] = new SelectList(_context.Tratamientos, "Id", "Nombre", sesion.TratamientoId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", sesion.UsuarioCreacionId);
            return View(sesion);
        }

        // GET: Sesiones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sesion = await _context.Sesiones.FindAsync(id);
            if (sesion == null)
            {
                return NotFound();
            }
            ViewData["PacienteId"] = new SelectList(_context.Paciente, "Id", "Apellido", sesion.PacienteId);
            ViewData["PatologiaId"] = new SelectList(_context.Patologias, "Id", "Nombre", sesion.PatologiaId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", sesion.ProductoId);
            ViewData["TratamientoId"] = new SelectList(_context.Tratamientos, "Id", "Nombre", sesion.TratamientoId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", sesion.UsuarioCreacionId);
            return View(sesion);
        }

        // POST: Sesiones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UsuarioCreacionId,Fecha,PacienteId,PatologiaId,TratamientoId,ProductoId,Peso,Presion,Observaciones,Operaciones,Medicacion,Automedicacion,DiagnosticoMedico")] Sesion sesion)
        {
            if (id != sesion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sesion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SesionExists(sesion.Id))
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
            ViewData["PacienteId"] = new SelectList(_context.Paciente, "Id", "Apellido", sesion.PacienteId);
            ViewData["PatologiaId"] = new SelectList(_context.Patologias, "Id", "Nombre", sesion.PatologiaId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", sesion.ProductoId);
            ViewData["TratamientoId"] = new SelectList(_context.Tratamientos, "Id", "Nombre", sesion.TratamientoId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", sesion.UsuarioCreacionId);
            return View(sesion);
        }

        // GET: Sesiones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sesion = await _context.Sesiones
                .Include(s => s.Paciente)
                .Include(s => s.Patologia)
                .Include(s => s.Producto)
                .Include(s => s.Tratamiento)
                .Include(s => s.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sesion == null)
            {
                return NotFound();
            }

            return View(sesion);
        }

        // POST: Sesiones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sesion = await _context.Sesiones.FindAsync(id);
            _context.Sesiones.Remove(sesion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SesionExists(int id)
        {
            return _context.Sesiones.Any(e => e.Id == id);
        }
    }
}
