using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TeraFrontendMVC.Models.Account;

namespace TeraFrontendMVC.Controllers
{
    public class AccountController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<AccountController> _logger;

        public AccountController(HttpClient httpClient, ILogger<AccountController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://web/api/Usuarios/registrar-usuario", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login", "Account");
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            return View(model);
        }

        // GET: Account/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://web/api/Usuarios/modificar-contrasena", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login", "Account");
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            // Asegúrate de devolver el modelo al regresar a la vista
            return View(model);
        }


        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                // Lógica de autenticación aquí
                return RedirectToAction("Index", "Home"); // Redirige a la página principal si es exitoso
            }

            return View(model); // Si hay errores, vuelve a mostrar el formulario
        }
    }
}
