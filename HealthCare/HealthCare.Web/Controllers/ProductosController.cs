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
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager; 
        public ProductosController(ApplicationDbContext context,UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {            
            var userId = _userManager.GetUserId(HttpContext.User);            
            var lista = await _context.Productos.Where(p => p.UsuarioCreacion.Id == userId && p.Activo == true).Include(u => u.UsuarioCreacion).ToListAsync();
            return View(lista);
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Precio,Id,Nombre,Activo,UsuarioCreacionId")] Producto producto)
        public async Task<IActionResult> Create([Bind("Precio,Id,Nombre")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                
                producto.UsuarioCreacionId = _userManager.GetUserId(HttpContext.User);
                producto.Activo = true;

                _context.Add(producto);
                await _context.SaveChangesAsync();

                //agregado
                TempData["mensaje"] = "Se agregó Tratamiento con éxito.";
                TempData["tipo"] = "alert-success";
                //-------------------------------------
                return RedirectToAction(nameof(Index));
            }
            //ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", producto.UsuarioCreacionId);
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            //ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", producto.UsuarioCreacionId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Precio,Id,Nombre,Activo,UsuarioCreacionId")] Producto producto)
        public async Task<IActionResult> Edit(int id, [Bind("Precio,Id,Nombre")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //agregado
                    producto.UsuarioCreacionId = _userManager.GetUserId(HttpContext.User);
                    producto.Activo = true;

                    _context.Update(producto);
                    await _context.SaveChangesAsync();

                    //agregado
                    TempData["mensaje"] = "Se modificó Tratamiento con éxito.";
                    TempData["tipo"] = "alert-success";
                    //----------------------------------------
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
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
            //ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Id", producto.UsuarioCreacionId);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.UsuarioCreacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            //agregado para poner como inactivo el registro sin eliminarlo. Baja lógica
            producto.Activo = false;
            _context.Update(producto);

            //_context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            //agregado
            TempData["mensaje"] = "Se eliminó Tratamiento con éxito.";
            TempData["tipo"] = "alert-success";

            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
