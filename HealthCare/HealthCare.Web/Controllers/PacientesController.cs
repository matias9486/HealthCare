using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthCare.Web.Data;
using HealthCare.Web.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Identity;

namespace HealthCare.Web.Controllers
{
    public class PacientesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        public PacientesController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Pacientes
        public async Task<IActionResult> Index()
        {            
            var userId = _userManager.GetUserId(HttpContext.User);
            var lista = await _context.Paciente.Include(u => u.UsuarioCreacion).Where(p => p.UsuarioCreacionId == userId && p.Activo == true).ToListAsync();
            return View(lista);
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Paciente
                .Include(p => p.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Pacientes/Create
        public IActionResult Create()
        {
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }

        // POST: Pacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Imagen,Apellido,Direccion,FechaNacimiento,Id,Nombre,Activo,UsuarioCreacionId")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                paciente.UsuarioCreacionId = _userManager.GetUserId(HttpContext.User);
                paciente.Activo = true;
                //agregado para ffoto
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        paciente.Imagen = dataStream.ToArray();
                    }                    
                }
                
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                //agregado
                TempData["mensaje"] = "Se agregó Paciente con éxito.";
                TempData["tipo"] = "alert-success";
                //-------------------------------------      
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", paciente.UsuarioCreacionId);
            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Paciente.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", paciente.UsuarioCreacionId);
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Imagen,Apellido,Direccion,FechaNacimiento,Id,Nombre,Activo,UsuarioCreacionId")] Paciente paciente)
        public async Task<IActionResult> Edit(int id, [Bind("Apellido,Direccion,FechaNacimiento,Id,Nombre,Activo,UsuarioCreacionId")] Paciente paciente)
        {
            if (id != paciente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //agregado para foto
                    if (Request.Form.Files.Count > 0)
                    {
                        IFormFile file = Request.Form.Files.FirstOrDefault();
                        using (var dataStream = new MemoryStream())
                        {
                            await file.CopyToAsync(dataStream);
                            paciente.Imagen = dataStream.ToArray();
                        }
                    }
                    //agregado
                    paciente.UsuarioCreacionId = _userManager.GetUserId(HttpContext.User);
                    paciente.Activo = true;

                    

                    _context.Update(paciente);
                    await _context.SaveChangesAsync();

                    //agregado
                    TempData["mensaje"] = "Se modificó Paciente con éxito.";
                    TempData["tipo"] = "alert-success";
                    //----------------------------------------                    

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.Id))
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
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", paciente.UsuarioCreacionId);
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Paciente
                .Include(p => p.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paciente = await _context.Paciente.FindAsync(id);

            //agregado para poner como inactivo el registro sin eliminarlo. Baja lógica
            paciente.Activo = false;
            _context.Update(paciente);

            //_context.Paciente.Remove(paciente);

            await _context.SaveChangesAsync();

            //agregado
            TempData["mensaje"] = "Se eliminó Paciente con éxito.";
            TempData["tipo"] = "alert-success";

            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
            return _context.Paciente.Any(e => e.Id == id);
        }
    }
}
