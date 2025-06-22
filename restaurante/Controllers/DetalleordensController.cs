using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using restaurante.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using restaurante.dbContext;


namespace restaurante.Controllers
{
    [Authorize]

    public class DetalleordensController : Controller
    {
        private readonly DbrestauranteContext _context;
        public DetalleordensController(DbrestauranteContext context)
        {
            _context = context;
        }

        // GET: Detalleordens
        public async Task<IActionResult> Index()
        {
            var dbrestauranteContext = _context.Detalleordens.Include(d => d.IdProductoNavigation);
            return View(await dbrestauranteContext.ToListAsync());
        }

        // GET: Detalleordens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var detalleorden = await _context.Detalleordens
                                                        .Include(d => d.IdProductoNavigation)
                                                        .Include(d => d.IdOrdenNavigation)
                                                        .FirstOrDefaultAsync(m => m.IdDetalleOrden == id);

            if (detalleorden == null)
                return NotFound();

            return View(detalleorden);
        }
      
        // GET: Detalleordens/Create
        public async Task<IActionResult> Create()
        {
            await ViewDataProductos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente, Orden orden, List<Detalleorden> detalleorden, Detallecomplemento detOrdenComplemento, Decimal TotalAPagar, Decimal DescuentoPedido)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (detalleorden.Count > 0)
                {
                    if (cliente.Nombre == null)
                        cliente.Nombre = "Anonimo";

                    var ExisteCliente = await _context.Clientes.FirstOrDefaultAsync(x => x.Cedula == cliente.Cedula);

                    if (ExisteCliente == null)
                    {
                        cliente.FidePuntos = 1;
                        _context.Clientes.Add(cliente);
                        await _context.SaveChangesAsync();
                        orden.IdCliente = cliente.IdCliente;
                    }
                    else
                    {
                        ExisteCliente.FidePuntos += 1;
                        await _context.SaveChangesAsync();
                        orden.IdCliente = ExisteCliente.IdCliente;
                    }

                    orden.IdEmpleado = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    orden.FechaOrden = DateOnly.FromDateTime(DateTime.Now);
                    orden.TiempoOrden = TimeOnly.FromDateTime(DateTime.Now);
                    orden.Descuento = DescuentoPedido;
                    orden.Total = TotalAPagar;

                    _context.Ordens.Add(orden);
                    await _context.SaveChangesAsync();

                    foreach (var item in detalleorden)
                    {
                        item.IdOrden = orden.IdOrden;
                        _context.Detalleordens.Add(item);

                        foreach (var item2 in item.Detallecomplementos!)
                        {
                            detOrdenComplemento.IdComplemento = item2.IdComplemento;
                            detOrdenComplemento.IdDetalleOrden = item.IdDetalleOrden;
                            _context.Detallecomplementos.Add(item2);
                        }
                    }
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    return RedirectToAction("Index", "Ordens");
                }
                else
                {
                    transaction.Rollback();
                    await ViewDataProductos();

                    TempData["error"] = "Debe Ingrear productos en el carrito de productos.";
                    return View();
                }
            }
        }
        public async Task ViewDataProductos()
        {
            var listaproductos = await _context.Productos.AsNoTracking().ToListAsync();

            var Hamburguesas = listaproductos.Where(x => x.IdCategoria == 1).Select(x => new
            {
                x.IdProducto,
                NombreCompleto = $"{x.Nombre} {x.Descripcion}",
                x.Precio,
                x.IdCategoria
            }).ToList();

            var bebidas = listaproductos.Where(x => x.IdCategoria == 2).Select(x => new
            {
                x.IdProducto,
                NombreCompleto = x.Nombre + " " + x.Descripcion,
                x.Precio,
                x.IdCategoria
            }).ToList();

            var Pollos = listaproductos.Where(x => x.IdCategoria == 3).Select(x => new
            {
                x.IdProducto,
                NombreCompleto = x.Nombre + " " + x.Descripcion,
                x.Precio,
                x.IdCategoria
            }).ToList();

            var extras = listaproductos.Where(x => x.IdCategoria == 4).Select(x => new
            {
                x.IdProducto,
                NombreCompleto = x.Nombre + " " + x.Descripcion,
                x.Precio,
                x.IdCategoria
            }).ToList();

            var listacomplementos = await _context.Complementos.AsNoTracking()
                                                                                        .Select(x => new
                                                                                        {
                                                                                            x.IdComplemento,
                                                                                            x.NombreIngrediente,
                                                                                            CategoriaNombre = x.IdCategoriaComplementoNavigation.TipoCategoriaComplemento
                                                                                        }).GroupBy(x => x.CategoriaNombre).ToListAsync();

            var items = await _context.Productos.AsNoTracking().Select(x => new
            {
                x.IdProducto,
                NombreCompleto = x.Nombre + " " + x.Descripcion,
                x.Precio,
                x.IdCategoria
            }).ToListAsync();

            ViewBag.ProductosConPrecios = items;

            ViewData["IdProducto"] = new SelectList(Hamburguesas, "IdProducto", "NombreCompleto");
            ViewData["listaBebidas"] = new SelectList(bebidas, "IdProducto", "NombreCompleto");
            ViewData["listaPollos"] = new SelectList(Pollos, "IdProducto", "NombreCompleto");
            ViewData["listaExtras"] = new SelectList(extras, "IdProducto", "NombreCompleto");

            ViewData["listacomplementos"] = listacomplementos;
        }
        // GET: Detalleordens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var detalleorden = await _context.Detalleordens.FindAsync(id);

            if (detalleorden == null)
                return NotFound();

            ViewData["IdProducto"] = new SelectList(await _context.Productos.ToListAsync(), "IdProducto", "IdProducto", detalleorden.IdProducto);
            return View(detalleorden);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDetalleOrden,IdOrden,Cantidad,PrecioUnitario,IdDetalleProducto,IdSalsa,IdQueso,IdProducto")] Detalleorden detalleorden)
        {
            if (id != detalleorden.IdDetalleOrden)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleorden);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleordenExists(detalleorden.IdDetalleOrden))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdProducto"] = new SelectList(await _context.Productos.ToListAsync(), "IdProducto", "IdProducto", detalleorden.IdProducto);
            return View(detalleorden);
        }

        // GET: Detalleordens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var detalleorden = await _context.Detalleordens
                .Include(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdDetalleOrden == id);

            if (detalleorden == null)
                return NotFound();

            return View(detalleorden);
        }

        // POST: Detalleordens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalleorden = await _context.Detalleordens.FindAsync(id);

            if (detalleorden != null)
                _context.Detalleordens.Remove(detalleorden);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalleordenExists(int id)
        {
            return _context.Detalleordens.Any(e => e.IdDetalleOrden == id);
        }
    }
}
