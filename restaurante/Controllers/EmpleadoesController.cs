using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using restaurante.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using restaurante.dbContext;

namespace restaurante.Controllers
{
    [Authorize(Roles = "2")]
    public class EmpleadoesController : Controller
    {

        private readonly DbrestauranteContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPasswordHasher<Empleado> _passwordHasher;

        public EmpleadoesController(DbrestauranteContext context, IWebHostEnvironment iWebHostEnviroment, IPasswordHasher<Empleado> passwordHasher)
        {
            _context = context;
            _webHostEnvironment = iWebHostEnviroment;
            _passwordHasher = passwordHasher;
        }

        // GET: Empleadoes
        public async Task<IActionResult> Index()
        {
            var dbrestauranteContext = _context.Empleados.AsNoTracking().Include(e => e.IdCargoNavigation).ToListAsync();
            return View(await dbrestauranteContext);
        }

        // GET: Empleadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var empleado = await _context.Empleados.AsNoTracking()
                .Include(e => e.IdCargoNavigation)
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);

            if (empleado == null)
                return NotFound();

            return View(empleado);
        }

        // GET: Empleadoes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdCargo"] = new SelectList(await _context.Cargos.ToListAsync(), "IdCargo", "TipoCargo");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            try
            {
                if (empleado.FotoEmpleadoFile != null)
                    await SubirFoto(empleado);

                if (empleado.Pass != null)
                    empleado.Pass = _passwordHasher.HashPassword(empleado, empleado.Pass);

                _context.Empleados.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewData["IdCargo"] = new SelectList(await _context.Cargos.ToListAsync(), "IdCargo", "TipoCargo", empleado.IdCargo);
                return View(empleado);
            }
        }

        private async Task SubirFoto(Empleado empleado)
        {
            //formar nombre del archivo foto
            string wwRootPath = _webHostEnvironment.WebRootPath;
            string extension = Path.GetExtension(empleado.FotoEmpleadoFile!.FileName);
            string nombreFoto = $"{empleado.Nombre+empleado.ApellidoPaterno}{extension}";

            empleado.FotoEmpleado = nombreFoto;

            //copiar la foto en el proyecto del servidor
            string path = Path.Combine($"{wwRootPath}/FotoUsuario/", nombreFoto);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await empleado.FotoEmpleadoFile.CopyToAsync(fileStream);
            }
        }

        // GET: Empleadoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)            
                return NotFound();

            var empleado = await _context.Empleados.FindAsync(id);

            if (empleado == null)
                return NotFound();

            ViewData["IdCargo"] = new SelectList(await _context.Cargos.ToListAsync(), "IdCargo", "TipoCargo", empleado.IdCargo);
            return View(empleado);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empleado empleado)
        {
            if (id != empleado.IdEmpleado)
                return NotFound();

            var EmpleadoEncontrado = await _context.Empleados.FirstOrDefaultAsync(x => x.IdEmpleado == id);

            if (EmpleadoEncontrado == null)
                return NotFound();

            empleado.Pass = EmpleadoEncontrado.Pass;

            if (empleado.FotoEmpleadoFile != null)
                await SubirFoto(empleado);
            else
            {
                if (string.IsNullOrEmpty(EmpleadoEncontrado.FotoEmpleado))
                    EmpleadoEncontrado.FotoEmpleado = null;
            }

            _context.Entry(EmpleadoEncontrado).CurrentValues.SetValues(empleado);
            try
            {               
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewData["IdCargo"] = new SelectList(await _context.Cargos.ToListAsync(), "IdCargo", "TipoCargo", empleado.IdCargo);
                return View(empleado);
            }
        }
        public IActionResult CambiarPassword(int? IdEmpleado)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarPassword(int id, Empleado empleado)
        {
            var objUsuario = await _context.Empleados.FirstOrDefaultAsync(x => x.IdEmpleado == id);

            if (objUsuario == null)
                return NotFound();

            objUsuario.Pass = _passwordHasher.HashPassword(empleado, empleado.Pass!);

            try
            {
                _context.Empleados.Attach(objUsuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch {
                return View();
            }
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var empleado = await _context.Empleados.AsNoTracking()
                                                                    .Include(e => e.IdCargoNavigation)
                                                                    .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            if (empleado == null)
                return NotFound();

            return View(empleado);
        }

        // POST: Empleadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);

            if (empleado != null)
                _context.Empleados.Remove(empleado);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.IdEmpleado == id);
        }
    }
}
