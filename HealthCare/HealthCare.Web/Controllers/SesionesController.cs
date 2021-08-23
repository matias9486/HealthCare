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
    public class SesionesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        public SesionesController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        
        //Accion para filtrar combos por tipo de patologia... FUNCIONA
        public IActionResult filtrarPorTipo(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var lista = _context.Patologias.Where(p => p.UsuarioCreacion.Id == userId && p.Activo == true && p.TipoId == id).OrderBy(p => p.Nombre).ToList();
                
            return Json(lista);
        }


        public async Task<IActionResult> Pacientes()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var lista = await _context.Paciente.Include(u => u.UsuarioCreacion).Where(p => p.UsuarioCreacionId == userId && p.Activo == true).ToListAsync();
            return View(lista);
        }

        // GET: Sesiones
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var applicationDbContext = _context.Sesiones.Include(s => s.Paciente).Include(s => s.Patologia).Include(s => s.Producto).Include(s => s.Tratamiento).Include(s => s.UsuarioCreacion).Where(p => p.UsuarioCreacionId == userId);
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
        public IActionResult Create(int id)
        {
            
            var userId = _userManager.GetUserId(HttpContext.User);
            //creo nueva lista de tipo SelectViewModel para poder mostrar en un combo mas de un atributo de pacientes
            List<SelectViewModel> pacientes = (from pac in _context.Paciente.Where(p => p.UsuarioCreacion.Id == userId && p.Activo == true).OrderBy(p=>p.Apellido)
                                               select new SelectViewModel()
                                                    {
                                                        ID = pac.Id,
                                                        Value = $"{pac.Apellido}, {pac.Nombre}"
                                                    }
                                                    ).ToList();


            List<Producto> listaProductos = _context.Productos.Where(p => p.UsuarioCreacion.Id == userId && p.Activo == true).OrderBy(p => p.Nombre).ToList();
            List<Tratamiento> listaTratamientos = _context.Tratamientos.Where(p => p.UsuarioCreacion.Id == userId && p.Activo == true).OrderBy(p => p.Nombre).ToList();

            //recupero los id de los tipos de patologias que se usaron en las patologias segun usuario y si esta activa
            List<int> TiposID = _context.Patologias.Where(p => p.UsuarioCreacion.Id == userId && p.Activo == true).Select(p => p.TipoId).Distinct().ToList();
            
            //tipo de patologias filtrada por los tipos que estan en uso.. funciona            
            List<TipoPatologia> listaTipo = _context.TipoPatologias.Where(p => p.UsuarioCreacion.Id == userId && p.Activo == true).Where(esta=> TiposID.Contains(esta.Id)).OrderBy(p => p.Nombre).ToList();

            //compruebo que esten cargadas las listas. ListaPatologias, ya se tiene en cuenta al filtrar listaTipo
            if (listaTipo.Count == 0 || listaProductos.Count == 0 || listaTratamientos.Count == 0 || pacientes.Count == 0)
            {
                TempData["mensaje"] = "Antes de agregar una sesión debe agregar tratamientos, productos, tipos de patologías, patologías y pacientes.";
                TempData["tipo"] = "alert-warning";
                return RedirectToAction("Index");
            }

            //obtengo primer id de la lista Tipos
            int idTipo = listaTipo.First().Id;
            //filtrada por el primer tipo de patologia
            List<Patologia> listaPatologias = _context.Patologias.Where(p => p.UsuarioCreacion.Id == userId && p.Activo == true && p.TipoId==idTipo).OrderBy(p => p.Nombre).ToList();
                                                            
            ViewBag.PacientesLista = new SelectList(pacientes, "ID", "Value");
            ViewBag.TipoPatologias = new SelectList(listaTipo, "Id", "Nombre");            
            ViewData["PatologiaId"] = new SelectList(listaPatologias, "Id", "Nombre");
            ViewData["ProductoId"] = new SelectList(listaProductos, "Id", "Nombre");
            ViewData["TratamientoId"] = new SelectList(listaTratamientos, "Id", "Nombre");
            
            if (_context.Sesiones.Where(t => t.UsuarioCreacion.Id == userId).Count() == 0)
            {
                TempData["mensaje"] = "Sesiones es el periodo de tiempo durante el cual el paciente es tratado por el profesional.";
                TempData["tipo"] = "alert-primary";
            }
                       
            return View();
        }

        // POST: Sesiones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioCreacionId,Fecha,PacienteId,PatologiaId,TratamientoId,ProductoId,Peso,Presion,Observaciones,Operaciones,Medicacion,Automedicacion,DiagnosticoMedico")] Sesion sesion)
        {
            sesion.UsuarioCreacionId = _userManager.GetUserId(HttpContext.User);
            
            if (ModelState.IsValid)
            {
                _context.Add(sesion);
                await _context.SaveChangesAsync();
                //agregado
                TempData["mensaje"] = "Se agregó Sesión con éxito.";
                TempData["tipo"] = "alert-success";

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
                    sesion.UsuarioCreacionId = _userManager.GetUserId(HttpContext.User);
                    _context.Update(sesion);
                    await _context.SaveChangesAsync();

                    //agregado
                    TempData["mensaje"] = "Se modificó Sesión con éxito.";
                    TempData["tipo"] = "alert-success";
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
