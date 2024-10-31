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

    public class SalsasController : Controller
    {
        private readonly DbrestauranteContext _context;

        public SalsasController(DbrestauranteContext context)
        {
            _context = context;
        }

        // GET: Salsas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Complementos.AsNoTracking().Where(x => x.IdCategoriaComplemento == 2).Include(x => x.IdCategoriaComplementoNavigation).ToListAsync());
        }

        // GET: Salsas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var salsa = await _context.Complementos.AsNoTracking().Include(x => x.IdCategoriaComplementoNavigation)
                .FirstOrDefaultAsync(m => m.IdComplemento == id);

            if (salsa == null)
                return NotFound();

            return View(salsa);
        }

        // GET: Salsas/Create
        public async Task<IActionResult> Create()
        {
            ViewData["listaSalsa"] = new SelectList(await _context.Categoriacomplementos.ToListAsync(), "IdCategoriaComplemento", "TipoCategoriaComplemento");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdComplemento,NombreIngrediente,IdCategoriaComplemento")] Complemento complemento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(complemento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["listaSalsa"] = new SelectList(await _context.Categoriacomplementos.ToListAsync(), "IdCategoriaComplemento", "TipoCategoriaComplemento");
            return View(complemento);
        }

        // GET: Salsas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            ViewData["listaSalsa"] = new SelectList(await _context.Categoriacomplementos.ToListAsync(), "IdCategoriaComplemento", "TipoCategoriaComplemento");
            var salsa = await _context.Complementos.FindAsync(id);

            if (salsa == null)
                return NotFound();

            return View(salsa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdComplemento,NombreIngrediente,IdCategoriaComplemento")] Complemento complemento)
        {
            if (id != complemento.IdComplemento)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(complemento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalsaExists(complemento.IdComplemento))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(complemento);
        }

        // GET: Salsas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var salsa = await _context.Complementos.Include(x => x.IdCategoriaComplementoNavigation)
                .FirstOrDefaultAsync(m => m.IdComplemento == id);

            if (salsa == null)
                return NotFound();

            return View(salsa);
        }

        // POST: Salsas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salsa = await _context.Complementos.FindAsync(id);

            if (salsa != null)
                _context.Complementos.Remove(salsa);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalsaExists(int id)
        {
            return _context.Complementos.Any(e => e.IdComplemento == id);
        }
    }
}
