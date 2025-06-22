using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using restaurante.dbContext;
using restaurante.Dtos;
using restaurante.Interfaces;
using restaurante.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restaurante.Controllers
{
    [Authorize]


    public class BebidumsController : Controller
    {
        private readonly IProducto _bebidumsService;

        public BebidumsController(IProducto bebidumsService)
        {
            _bebidumsService = bebidumsService;
        }

        // GET: Bebidums
        public async Task<ActionResult<List<ProductoDto>>> Index(int? id)
        {
            var dbrestauranteContext = await _bebidumsService.GetProducto(id);
            return View(dbrestauranteContext);
        }

        // GET: Bebidums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)            
                return NotFound();

            var bebidum = await _bebidumsService.Details(id);

            if (bebidum == null)            
                return NotFound();           

            return View(bebidum);
        }

        // GET: Bebidums/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdCategoria"] = new SelectList(await _bebidumsService.GetCategoria(), "IdCategoria", "TipoCategoria");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,Nombre,Descripcion,IdCategoria,Precio,FotoProducto")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                await _bebidumsService.CrearProducto(producto);
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCategoria"] =  new SelectList(await _bebidumsService.GetCategoria(), "IdCategoria", "TipoCategoria", producto.IdCategoria);
            return View(producto);
        }

        // GET: Bebidums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var producto = await _bebidumsService.Details(id);

            if (producto == null)
                return NotFound();

            ViewData["IdCategoria"] = new SelectList(await _bebidumsService.GetCategoria(), "IdCategoria", "TipoCategoria", producto.IdCategoria);
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
                var resultado = await _bebidumsService.ModificarProducto(id,producto);
                return resultado ? RedirectToAction(nameof(Index)) : NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(await _bebidumsService.GetCategoria(), "IdCategoria", "TipoCategoria", producto.IdCategoria);
            return View(producto);
        }

        // GET: Bebidums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var bebidum = await _bebidumsService.Details(id);

            if (bebidum == null)
                return NotFound();

            return View(bebidum);
        }

        // POST: Bebidums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bebidum = await _bebidumsService.EliminarProducto(id);

            return bebidum ? RedirectToAction(nameof(Index)) : NotFound();
        }

    }
}
