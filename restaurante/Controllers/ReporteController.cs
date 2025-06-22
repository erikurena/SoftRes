using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Table = iText.Layout.Element.Table;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using restaurante.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using restaurante.dbContext;

namespace restaurante.Controllers
{
    public class ReporteController : Controller
    {
        public readonly DbrestauranteContext _dbContext;

        public ReporteController(DbrestauranteContext conexto)
        {
            _dbContext = conexto;
        }
        // GET: ReporteController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReporteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReporteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReporteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Reporte reporte)
        {
            if(ModelState.IsValid)
            {
                var listaProductos = await _dbContext.Detalleordens.AsNoTracking()
                                                                             .Where(x => x.IdOrdenNavigation.FechaOrden >= reporte.FechaInicio && x.IdOrdenNavigation.FechaOrden <= reporte.FechaFin)
                                                                             .Select(x => new
                                                                             {
                                                                                 x.IdProductoNavigation.Nombre,
                                                                                 x.Cantidad,
                                                                                 x.PrecioUnitario,
                                                                                 x.SubTotal,
                                                                                 x.IdOrdenNavigation.FechaOrden,
                                                                                 x.IdOrdenNavigation.TiempoOrden
                                                                             }).ToListAsync();

                using (MemoryStream ms = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(ms);
                    PdfDocument pdf = new PdfDocument(writer);
                    Document document = new Document(pdf);

                    //añadir documento
                    document.Add(new Paragraph("Lista de Productos Vendidos").SetFontSize(18).SetBold().SetTextAlignment(TextAlignment.CENTER));

                    document.Add(new Paragraph("Fecha de Inicio:"+reporte.FechaInicio).SetFontSize(12).SetTextAlignment(TextAlignment.LEFT).SetMarginBottom(0));
                    document.Add(new Paragraph("Fecha de Finalización:" + reporte.FechaFin).SetFontSize(12).SetTextAlignment(TextAlignment.LEFT).SetMarginTop(0));

                    // Crear una tabla con 7 columnas (por ejemplo: ID, Nombre, Precio)
                    Table table = new Table(7).SetTextAlignment(TextAlignment.CENTER);
                    table.SetWidth(UnitValue.CreatePercentValue(100)); // Ancho al 100%

                    // Añadir encabezados de columna
                    table.AddHeaderCell("Nº");
                    table.AddHeaderCell("Nombre");
                    table.AddHeaderCell("Cantidad");
                    table.AddHeaderCell("Precio");
                    table.AddHeaderCell("SubTotal");
                    table.AddHeaderCell("Fecha");
                    table.AddHeaderCell("Hora");

                    var count = 1;
                    int? totalCantidad = 0;
                    decimal? totalProductos = 0;
                    // Llenar la tabla con los datos de la lista de productos
                    foreach (var producto in listaProductos)
                    {
                        table.AddCell((count++).ToString());
                        table.AddCell(producto.Nombre);
                        table.AddCell(producto.Cantidad.ToString());
                        table.AddCell(producto.PrecioUnitario.ToString());
                        table.AddCell(producto.SubTotal.ToString());
                        table.AddCell(producto.FechaOrden.ToString());
                        table.AddCell(producto.TiempoOrden.ToString());

                        totalCantidad += producto.Cantidad;
                        totalProductos += producto.SubTotal;
                    }

                    // Añadir la tabla al documento
                    document.Add(table);

                    document.Add(new Paragraph("Total de Productos: " + totalCantidad  ).SetFontSize(12).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetMarginBottom(0));
                    document.Add(new Paragraph("Total Vendido: " + totalProductos).SetFontSize(12).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetMarginTop(0));

                    document.Close();

                    // Regresar el PDF como un archivo descargable
                    return File(ms.ToArray(), "application/pdf", "ProductosVendidos-"+reporte.FechaInicio+"-"+ reporte.FechaFin+".pdf");
                }
            }
           else
            {
                return View(reporte);
            }
        }

        public  IActionResult ProductosMasVendidos()
        {
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> ProductosMasVendidos(Reporte reporte)
        {
            var listaProductosVendidos = await _dbContext.Detalleordens.AsNoTracking()
                                                                   .Where(dx => dx.IdOrdenNavigation.FechaOrden >= reporte.FechaInicio && dx.IdOrdenNavigation.FechaOrden <= reporte.FechaFin)
                                                                   .GroupBy(dx => dx.IdProductoNavigation.Nombre)
                                                                   .Select(x => new
                                                                   {
                                                                       Nombre = x.Key,
                                                                       Cantidad = x.Sum(dx => dx.Cantidad),
                                                                       SubTotal = x.Sum(dx => dx.SubTotal),
                                                                   }).OrderByDescending(x => x.Cantidad).ToListAsync();

            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                //añadir documento
                document.Add(new Paragraph("Lista de Productos Mas Vendidos").SetFontSize(18).SetBold().SetTextAlignment(TextAlignment.CENTER));

                document.Add(new Paragraph("Fecha de Inicio:" + reporte.FechaInicio).SetFontSize(12).SetTextAlignment(TextAlignment.LEFT).SetMarginBottom(0));
                document.Add(new Paragraph("Fecha de Finalización:" + reporte.FechaFin).SetFontSize(12).SetTextAlignment(TextAlignment.LEFT).SetMarginTop(0));

                // Crear una tabla con 4 columnas (por ejemplo: ID, Nombre, Precio)
                Table table = new Table(4).SetTextAlignment(TextAlignment.CENTER);
                table.SetWidth(UnitValue.CreatePercentValue(100)); // Ancho al 100%

                // Añadir encabezados de columna
                table.AddHeaderCell("Nº");
                table.AddHeaderCell("Nombre");
                table.AddHeaderCell("Cantidad");
                table.AddHeaderCell("SubTotal");

                var count = 1;
                int? totalCantidad = 0;
                decimal? totalProductos = 0;

                // Llenar la tabla con los datos de la lista de productos
                foreach (var producto in listaProductosVendidos)
                {
                    table.AddCell((count++).ToString());
                    table.AddCell(producto.Nombre);
                    table.AddCell(producto.Cantidad.ToString());
                    table.AddCell(producto.SubTotal.ToString());
                }

                // Añadir la tabla al documento
                document.Add(table);

                document.Close();

                // Regresar el PDF como un archivo descargable
                return File(ms.ToArray(), "application/pdf", "ProductosMasVendidos-" + reporte.FechaInicio + "-" + reporte.FechaFin + ".pdf");
            }
        }

        // GET: ReporteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
       
      
        // POST: ReporteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReporteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReporteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
