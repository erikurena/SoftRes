using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using restaurante.Models;
using Microsoft.AspNetCore.Authorization;
using restaurante.dbContext;

namespace restaurante.Controllers
{
    [Authorize]

    public class BebidumsController : Controller
    {
        private readonly DbrestauranteContext _context;

        public BebidumsController(DbrestauranteContext context)
        {
            _context = context;
        }

        // GET: Bebidums
        public async Task<IActionResult> Index()
        {
            var dbrestauranteContext = _context.Productos.Include(b => b.IdCategoriaNavigation).Where( b => b.IdCategoria == 2);
            return View(await dbrestauranteContext.ToListAsync());
        }

        // GET: Bebidums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bebidum = await _context.Productos
                .Include(b => b.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (bebidum == null)
            {
                return NotFound();
            }

            return View(bebidum);
        }

        // GET: Bebidums/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdCategoria"] = new SelectList(await _context.Categoria.ToListAsync(), "IdCategoria", "TipoCategoria");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,Nombre,Descripcion,IdCategoria,Precio,FotoProducto")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCategoria"] =  new SelectList(await _context.Categoria.ToListAsync(), "IdCategoria", "TipoCategoria", producto.IdCategoria);
            return View(producto);
        }

        // GET: Bebidums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound();

            ViewData["IdCategoria"] = new SelectList(await _context.Categoria.ToListAsync(), "IdCategoria", "TipoCategoria", producto.IdCategoria);
            return View(producto);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,Nombre,Descripcion,IdCategoria,Precio,FotoProducto")] Producto producto)
        {
            if (id != producto.IdProducto)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BebidumExists(producto.IdProducto))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(await _context.Categoria.ToListAsync(), "IdCategoria", "TipoCategoria", producto.IdCategoria);
            return View(producto);
        }

        // GET: Bebidums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var bebidum = await _context.Productos
                .Include(b => b.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);

            if (bebidum == null)
                return NotFound();

            return View(bebidum);
        }

        // POST: Bebidums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bebidum = await _context.Productos.FindAsync(id);

            if (bebidum != null)
            {
                _context.Productos.Remove(bebidum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BebidumExists(int id)
        {
            return _context.Productos.Any(e => e.IdProducto == id);
        }
    }
}
