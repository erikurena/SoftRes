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

    public class SalsasController : Controller
    {
        private readonly IComplemento _compleService;

        public SalsasController(IComplemento compleService)
        {
            _compleService = compleService;
        }

        // GET: Salsas
        public async Task<IActionResult> Index(int id)
        {
            return View(await _compleService.GetAllComplementos(id));
        }

        // GET: Salsas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var salsa = await _compleService.GetComplementoById(id);

            if (salsa == null)
                return NotFound();

            return View(salsa);
        }

        // GET: Salsas/Create
        public async Task<IActionResult> Create()
        {
            ViewData["listaSalsa"] = new SelectList(await _compleService.GetAllCategoriasComplemento(), "IdCategoriaComplemento", "TipoCategoriaComplemento");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdComplemento,NombreIngrediente,IdCategoriaComplemento")] Complemento complemento)
        {
            if (ModelState.IsValid)
            {
                await _compleService.CreateComplemento(complemento);
                return RedirectToAction(nameof(Index), new { id = 2 });
            }
            ViewData["listaSalsa"] = new SelectList(await _compleService.GetAllCategoriasComplemento(), "IdCategoriaComplemento", "TipoCategoriaComplemento");
            return View(complemento);
        }

        // GET: Salsas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            ViewData["listaSalsa"] = new SelectList(await _compleService.GetAllCategoriasComplemento(), "IdCategoriaComplemento", "TipoCategoriaComplemento");
            var salsa = await _compleService.GetComplementoById(id);

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
                 var resultado = await  _compleService.UpdateComplemento(id, complemento);
                   
                return resultado ? RedirectToAction(nameof(Index), new { id = 2 }) : NotFound();
            }
            return View(complemento);
        }

        // GET: Salsas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var salsa = await _compleService.GetComplementoById(id);

            if (salsa == null)
                return NotFound();

            return View(salsa);
        }

        // POST: Salsas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salsa = await _compleService.DeleteComplemento(id);

            return salsa ? RedirectToAction(nameof(Index), new { id = 2 }) : NotFound();
        }
    }
}
