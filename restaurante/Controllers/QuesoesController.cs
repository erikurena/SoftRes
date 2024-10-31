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

    public class QuesoesController : Controller
    {
        private readonly DbrestauranteContext _context;

        public QuesoesController(DbrestauranteContext context)
        {
            _context = context;
        }

        // GET: Quesoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Complementos.Where(x => x.IdCategoriaComplemento == 1).Include(x => x.IdCategoriaComplementoNavigation).ToListAsync());
        }

        // GET: Quesoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var queso = await _context.Complementos.AsNoTracking().Include(x => x.IdCategoriaComplementoNavigation)
                .FirstOrDefaultAsync(m => m.IdComplemento == id);

            if (queso == null)
                return NotFound();

            return View(queso);
        }

        // GET: Quesoes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["listaQueso"] = new SelectList(await _context.Categoriacomplementos.ToListAsync(), "IdCategoriaComplemento", "TipoCategoriaComplemento");
            return View();
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdComplemento,NombreIngrediente,IdCategoriaComplemento")] Complemento complemento)
        {
            if (ModelState.IsValid)
            {
                _context.Complementos.Add(complemento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["listaQueso"] = new SelectList(await _context.Categoriacomplementos.ToListAsync(), "IdCategoriaComplemento", "TipoCategoriaComplemento");
            return View(complemento);
        }

        // GET: Quesoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            ViewData["listaQueso"] = new SelectList(await _context.Categoriacomplementos.ToListAsync(), "IdCategoriaComplemento", "TipoCategoriaComplemento");
            var queso = await _context.Complementos.FindAsync(id);

            if (queso == null)
                return NotFound();

            return View(queso);
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
                    if (!QuesoExists(complemento.IdComplemento))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(complemento);
        }

        // GET: Quesoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var queso = await _context.Complementos.Include(x => x.IdCategoriaComplementoNavigation)
                .FirstOrDefaultAsync(m => m.IdComplemento == id);

            if (queso == null)
                return NotFound();

            return View(queso);
        }

        // POST: Quesoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var queso = await _context.Complementos.FindAsync(id);

            if (queso != null)
                _context.Complementos.Remove(queso);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuesoExists(int id)
        {
            return _context.Complementos.Any(e => e.IdComplemento == id);
        }
    }
}
