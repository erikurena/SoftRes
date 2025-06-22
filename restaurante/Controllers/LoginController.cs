using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using restaurante.Models;
using restaurante.dbContext;

namespace restaurante.Controllers
{
    public class LoginController : Controller
    {
        public readonly DbrestauranteContext? _contexto;
        private readonly IPasswordHasher<Empleado> _passwordHasher;
        public LoginController(DbrestauranteContext contexto, IPasswordHasher<Empleado> passwordHasher)
        {
            _contexto = contexto;
            _passwordHasher = passwordHasher;
        }
       
        public ActionResult Index()
        {
            return View();
        }       
        public ActionResult Login()
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string usuario, string password)
        {

            var UsuarioValido = await _contexto!.Empleados.FirstOrDefaultAsync(x => x.ApellidoPaterno == usuario);
           
            if (UsuarioValido != null)
            {
                if (UsuarioValido.FotoEmpleado == null)
                {
                    UsuarioValido.FotoEmpleado = "";
                }

                // Verificar la contraseña ingresada con la almacenada
                var result = _passwordHasher.VerifyHashedPassword(UsuarioValido, UsuarioValido.Pass, password);

                if (result == PasswordVerificationResult.Success )
                {
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, UsuarioValido.IdEmpleado.ToString()),
                            new Claim(ClaimTypes.Name, UsuarioValido.Nombre + " "+ UsuarioValido.ApellidoPaterno),
                            new Claim(ClaimTypes.Role, UsuarioValido.IdCargo.ToString()),
                            new Claim(ClaimTypes.GivenName, UsuarioValido.FotoEmpleado!)
                        };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["error"] = "Error en la cuenta o contraseña.";
                    return View();
                }              
            }
            else
            {
                TempData["error"] = "Error en la cuenta o contraseña.";
                return View();
            }
        }
        public IActionResult MostrarImagen()
        {
            var imgUsuario = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;

            if (string.IsNullOrEmpty(imgUsuario))
            {
                return NotFound("La imagen no existe.");
            }

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgUsuario);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound("La imagen no existe.");
            }

            ViewBag.ImgUsuario = imgUsuario;
            return View();
        }

        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}
