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
    public class OrdensController : Controller
    {
        private readonly DbrestauranteContext _context;

        public OrdensController(DbrestauranteContext context)
        {
            _context = context;
        }

        // GET: Ordens
        public async Task<IActionResult> Index()
        {
            var dbrestauranteContext = _context.Ordens
                                                                .Include(o => o.IdClienteNavigation)
                                                                .Include(o => o.IdEmpleadoNavigation)
                                                                .OrderByDescending(o => o.FechaOrden)
                                                                .ThenByDescending(o => o.TiempoOrden);

            return View(await dbrestauranteContext.ToListAsync());
        }

        // GET: Ordens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var orden = await _context.Ordens.AsNoTracking()
                                            .Include(o => o.IdClienteNavigation)
                                            .Include(o => o.IdEmpleadoNavigation)
                                            .Include(o => o.Detalleordens)
                                                .ThenInclude(d => d.IdProductoNavigation)
                                            .FirstOrDefaultAsync(m => m.IdOrden == id);

            if (orden == null)
                return NotFound();

            return View(orden);
        }

        // GET: Ordens/Create
        public  IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Orden orden,Cliente cliente,List<Detalleorden> detalleorden)
        {
            return View();
        }

        // GET: Ordens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var orden = await _context.Ordens.FindAsync(id);

            if (orden == null)
                return NotFound();

            ViewData["IdCliente"] = new SelectList(await _context.Clientes.ToListAsync(), "IdCliente", "IdCliente", orden.IdCliente);
            ViewData["IdEmpleado"] = new SelectList(await _context.Empleados.ToListAsync(), "IdEmpleado", "IdEmpleado", orden.IdEmpleado);
            return View(orden);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdOrden,IdCliente,IdEmpleado,FechaOrden,Total,TiempoOrden")] Orden orden)
        {
            if (id != orden.IdOrden)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orden);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdenExists(orden.IdOrden))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCliente"] = new SelectList(await _context.Clientes.ToListAsync(), "IdCliente", "IdCliente", orden.IdCliente);
            ViewData["IdEmpleado"] = new SelectList(await _context.Empleados.ToListAsync(), "IdEmpleado", "IdEmpleado", orden.IdEmpleado);
            return View(orden);
        }

        // GET: Ordens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var orden = await _context.Ordens
                .Include(o => o.IdClienteNavigation)
                .Include(o => o.IdEmpleadoNavigation)
                .FirstOrDefaultAsync(m => m.IdOrden == id);

            if (orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }

        // POST: Ordens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orden = await _context.Ordens.FindAsync(id);
            var cliente = await _context.Clientes.FindAsync(orden.IdCliente);

            if (orden != null && cliente != null)
            {
                if (cliente.FidePuntos > 1)
                {
                    cliente.FidePuntos -= 1;
                    _context.Ordens.Remove(orden);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Ordens.Remove(orden);
                    _context.Clientes.Remove(cliente);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OrdenExists(int id)
        {
            return _context.Ordens.Any(e => e.IdOrden == id);
        }
    }
}
