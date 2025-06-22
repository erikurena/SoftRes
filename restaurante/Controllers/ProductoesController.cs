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
using restaurante.Dtos;


namespace restaurante.Controllers
{
    [Authorize]

    public class ProductoesController : Controller
    {
        private readonly IProducto _productoService;

        public ProductoesController(IProducto productoService)
        {
            _productoService = productoService;
        }

        // GET: Productoes
        [HttpGet]
        public async Task<ActionResult<List<ProductoDto>>> Index(int? id)
        {
            var producto =  await _productoService.GetProducto(id);
            return View(producto);
        }

        // GET: Productoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var producto = await _productoService.Details(id.Value);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        // GET: Productoes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdCategoria"] = new SelectList(await _productoService.GetCategoria(), "IdCategoria", "TipoCategoria");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,Nombre,Descripcion,IdCategoria,Precio")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _productoService.CrearProducto(producto);
                if (!resultado)
                {
                    ViewData["IdCategoria"] = new SelectList(await _productoService.GetCategoria(), "IdCategoria", "TipoCategoria", producto.IdCategoria);
                    return View(producto);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(await _productoService.GetCategoria(), "IdCategoria", "TipoCategoria", producto.IdCategoria);
            return View(producto);
        }

        // GET: Productoes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
                return NotFound();

            var producto = await _productoService.Details(id);

            if (producto == null)
                return NotFound();

            ViewData["IdCategoria"] = new SelectList(await _productoService.GetCategoria(), "IdCategoria", "TipoCategoria", producto.IdCategoria);
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,Nombre,Descripcion,IdCategoria,Precio")] Producto producto)
        {
            if (id != producto.IdProducto)
                return NotFound();

            if (ModelState.IsValid)
            {
                var resutltado = await _productoService.ModificarProducto(id, producto);
                return resutltado ? RedirectToAction(nameof(Index)) : NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(await _productoService.GetCategoria(), "IdCategoria", "TipoCategoria", producto.IdCategoria);
            return View(producto);
        }

        // GET: Productoes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
                return NotFound();

            var producto = await _productoService.Details(id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _productoService.EliminarProducto(id);

            return producto ? RedirectToAction(nameof(Index)) : NotFound();
        }

    }
}
