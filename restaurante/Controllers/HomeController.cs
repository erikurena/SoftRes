using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using restaurante.dbContext;
using restaurante.Models;
using System.Diagnostics;
using System.Text.Json;



namespace restaurante.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DbrestauranteContext _contexto;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DbrestauranteContext contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }

        public async Task<IActionResult> Index()
        {
            var productosMasVendidos = await _contexto.Detalleordens
                                                                                    .Include(x => x.IdProductoNavigation)
                                                                                    .GroupBy(x => new { x.IdProductoNavigation.IdProducto, x.IdProductoNavigation.Nombre })
                                                                                    .Select(d => new
                                                                                    {
                                                                                        Total = d.Sum(d => d.Cantidad),
                                                                                        d.Key.Nombre
                                                                                    })
                                                                                    .OrderByDescending(x => x.Total).ToListAsync();

            var productosData = productosMasVendidos.Select(p => new
            {
                NombreProducto = p.Nombre,  
                value = p.Total   
            });

            var VendidosJSon = JsonSerializer.Serialize(productosData);

            var totalVendido = await _contexto.Detalleordens.AsNoTracking().SumAsync(x => x.Cantidad);

            ViewBag.TotalVendido = totalVendido;
            ViewData["ProductosMasVendidos"] = VendidosJSon;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
