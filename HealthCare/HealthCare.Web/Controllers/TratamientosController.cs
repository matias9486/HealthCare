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
    public class TratamientosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager; //servirá para guardar mi usuario

        //se agrego atributo userManager al constructor para tener el usuario que esta en sesion
        public TratamientosController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;// uso el atributo y parametro que agregue para tener el usuario logueado
        }

        // GET: Tratamientos
        public async Task<IActionResult> Index()
        {
            //a partir del _userManager obtento el id del usuario logueado
            var userId = _userManager.GetUserId(HttpContext.User);
            //filtro los tratamientos por el id del usuario y tratamiento activo y que traiga a su vez el atributo Usuario con sus datos 
            var lista = await _context.Tratamientos.Where(p=>p.UsuarioCreacion.Id==userId && p.Activo==true).Include(u=>u.UsuarioCreacion).ToListAsync();
            return View(lista);
        }

        // GET: Tratamientos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamientos
                .Include(t => t.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tratamiento == null)
            {
                return NotFound();
            }

            return View(tratamiento);
        }

        // GET: Tratamientos/Create
        public IActionResult Create()
        {
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }

        // POST: Tratamientos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Nombre,Precio,Activo,UsuarioCreacionId")] Tratamiento tratamiento)
        public async Task<IActionResult> Create([Bind("Id,Nombre,Precio")] Tratamiento tratamiento)
        {
            if (ModelState.IsValid)
            {
                //al atributo UsuarioCreacionId, le asigno el id del usuario logueado y creo como activo el tratamiento
                tratamiento.UsuarioCreacionId = _userManager.GetUserId(HttpContext.User);
                tratamiento.Activo = true;

                _context.Add(tratamiento);
                await _context.SaveChangesAsync();
                //agregado
                TempData["mensaje"] = "Se agregó Tratamiento con éxito.";
                TempData["tipo"] = "alert-success";
                //-------------------------------------
                return RedirectToAction(nameof(Index));
            }
            //ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", tratamiento.UsuarioCreacionId);
            return View(tratamiento);
        }

        // GET: Tratamientos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamientos.FindAsync(id);
            if (tratamiento == null)
            {
                return NotFound();
            }
            //ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", tratamiento.UsuarioCreacionId);
            return View(tratamiento);
        }

        // POST: Tratamientos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Precio,Activo,UsuarioCreacionId")] Tratamiento tratamiento)
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Precio")] Tratamiento tratamiento)
        {
            if (id != tratamiento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //al atributo UsuarioCreacionId, le asigno el id del usuario logueado y dejo como activo el tratamiento
                    tratamiento.UsuarioCreacionId = _userManager.GetUserId(HttpContext.User);
                    tratamiento.Activo = true;
                    _context.Update(tratamiento);
                    await _context.SaveChangesAsync();

                    //agregado
                    TempData["mensaje"] = "Se modificó Tratamiento con éxito.";
                    TempData["tipo"] = "alert-success";
                    //----------------------------------------
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TratamientoExists(tratamiento.Id))
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
            //ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", tratamiento.UsuarioCreacionId);
            return View(tratamiento);
        }

        // GET: Tratamientos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamientos
                .Include(t => t.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tratamiento == null)
            {
                return NotFound();
            }

            return View(tratamiento);
        }

        // POST: Tratamientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tratamiento = await _context.Tratamientos.FindAsync(id);

            //agregado para poner como inactivo el registro sin eliminarlo. Baja lógica
            tratamiento.Activo = false;
            _context.Update(tratamiento);
            //..........
            //_context.Tratamientos.Remove(tratamiento);  //eliminar registro
            await _context.SaveChangesAsync();

            //agregado
            TempData["mensaje"] = "Se eliminó Tratamiento con éxito.";
            TempData["tipo"] = "alert-success";
            return RedirectToAction(nameof(Index));

            //Una expresión nameof genera el nombre de una variable, un tipo o un miembro como constante de cadena. Muestra como string
        }

        

        private bool TratamientoExists(int id)
        {
            return _context.Tratamientos.Any(e => e.Id == id);
        }
    }
}
