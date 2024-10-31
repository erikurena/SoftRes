using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace restaurante.Controllers
{
    public class ComplementoController : Controller
    {
        [Authorize]

        // GET: ComplementoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ComplementoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ComplementoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ComplementoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: ComplementoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ComplementoController/Edit/5
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

        // GET: ComplementoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ComplementoController/Delete/5
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
