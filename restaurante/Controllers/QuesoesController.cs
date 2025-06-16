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
using restaurante.Interfaces;

namespace restaurante.Controllers
{
    [Authorize]

    public class QuesoesController : Controller
    {
        private readonly IComplemento _compleService;

        public QuesoesController(IComplemento complemento)
        {
            _compleService = complemento;
        }

        // GET: Quesoes
        public async Task<IActionResult> Index(int id)
        {
            return View(await _compleService.GetAllComplementos(id));
        }

        // GET: Quesoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var queso = await _compleService.GetComplementoById(id);

            if (queso == null)
                return NotFound();

            return View(queso);
        }

        // GET: Quesoes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["listaQueso"] = new SelectList(await _compleService.GetAllCategoriasComplemento(), "IdCategoriaComplemento", "TipoCategoriaComplemento");
            return View();
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdComplemento,NombreIngrediente,IdCategoriaComplemento")] Complemento complemento)
        {
            if (ModelState.IsValid)
            {
                await _compleService.CreateComplemento(complemento);
                return RedirectToAction(nameof(Index), new { id = 1 });
            }

            ViewData["listaQueso"] = new SelectList(await _compleService.GetAllCategoriasComplemento(), "IdCategoriaComplemento", "TipoCategoriaComplemento");
            return View(complemento);
        }

        // GET: Quesoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            ViewData["listaQueso"] = new SelectList(await _compleService.GetAllCategoriasComplemento(), "IdCategoriaComplemento", "TipoCategoriaComplemento");
            var queso = await _compleService.GetComplementoById(id);

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
                    var resultado = await _compleService.UpdateComplemento(id, complemento);
                   return resultado ? RedirectToAction(nameof(Index),new {id = 1}) : NotFound();
            }
            return View(complemento);
        }

        // GET: Quesoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var queso = await _compleService.GetComplementoById(id);

            if (queso == null)
                return NotFound();

            return View(queso);
        }

        // POST: Quesoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var queso = await _compleService.DeleteComplemento(id);

            return queso ? RedirectToAction(nameof(Index), new { id = 1 }) : View();
        }
    }
}
