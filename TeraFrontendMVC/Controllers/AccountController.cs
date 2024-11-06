using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeraFrontendMVC.Models.Account;

namespace TeraFrontendMVC.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                // Lógica de autenticación aquí
                return RedirectToAction("Index", "Home"); // Redirige a la página principal si es exitoso
            }

            return View(model); // Si hay errores, vuelve a mostrar el formulario
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(IFormCollection collection)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(IFormCollection collection)
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
