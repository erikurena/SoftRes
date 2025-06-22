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
using restaurante.Interfaces;


namespace restaurante.Controllers
{
    [Authorize(Roles = "2")]
    public class EmpleadoesController : Controller
    {
        private readonly IEmpleado _eService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPasswordHasher<Empleado> _passwordHasher;

        public EmpleadoesController(IEmpleado eservice, IWebHostEnvironment iWebHostEnviroment, IPasswordHasher<Empleado> passwordHasher)
        {
            _eService = eservice;
            _webHostEnvironment = iWebHostEnviroment;
            _passwordHasher = passwordHasher;
        }

        // GET: Empleadoes
        public async Task<IActionResult> Index()
        {
            var dbrestauranteContext = await _eService.GetEmpleados();
            return View(dbrestauranteContext);
        }

        // GET: Empleadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var empleado = await _eService.GetEmpleadoById(id);

            if (empleado == null)
                return NotFound();

            return View(empleado);
        }

        // GET: Empleadoes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdCargo"] = new SelectList(await _eService.Cargos(), "IdCargo", "TipoCargo");
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

                await _eService.CreateEmpleado(empleado);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewData["IdCargo"] = new SelectList(await _eService.Cargos(), "IdCargo", "TipoCargo", empleado.IdCargo);
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

            var empleado = await _eService.GetEmpleadoById(id);

            if (empleado == null)
                return NotFound();

            ViewData["IdCargo"] = new SelectList(await _eService.Cargos(), "IdCargo", "TipoCargo", empleado.IdCargo);
            return View(empleado);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empleado empleado)
        {
            if (id != empleado.IdEmpleado)
                return NotFound();

            var EmpleadoEncontrado = await _eService.GetEmpleadoById(id);

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

           var resultado = await _eService.UpdateEmpleado(id, empleado);
            if (resultado)
            {               
                return RedirectToAction(nameof(Index));
            }
            else 
            {
                ViewData["IdCargo"] = new SelectList(await _eService.Cargos(), "IdCargo", "TipoCargo", empleado.IdCargo);
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
            if (id != empleado.IdEmpleado)
                return NotFound();

            try
            {
                await _eService.CambiarPassword(id, empleado);
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

            var empleado = await _eService.GetEmpleadoById(id);

            if (empleado == null)
                return NotFound();

            return View(empleado);
        }

        // POST: Empleadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _eService.DeleteEmpleado(id);

            return empleado ? RedirectToAction(nameof(Index)) : NotFound();
        }

    }
}
