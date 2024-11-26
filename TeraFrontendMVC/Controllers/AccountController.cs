using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TeraFrontendMVC.Filter;
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
        [ServiceFilter(typeof(RedirectIfAuthenticatedFilter))]
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
                    TempData["SucessMessage"] = "Usuario registrado con éxito";
                    return RedirectToAction("Login", "Account");
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            return View(model);
        }

        // GET: Account/ChangePassword
        [HttpGet]
        [ServiceFilter(typeof(RedirectIfNotAuthenticatedFilter))]
        public IActionResult ChangePassword()
        {
            var checkToken = HttpContext.Session.GetString("AuthToken");

            if (string.IsNullOrEmpty(checkToken))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var changpwdjson = JsonConvert.SerializeObject(model);
                var changepwdcontent = new StringContent(changpwdjson, Encoding.UTF8, "application/json");
                var responseChangpwd = await _httpClient.PostAsync("http://web/api/Usuarios/modificar-contrasena", changepwdcontent);

                if (responseChangpwd.IsSuccessStatusCode)
                {

                    var token = HttpContext.Session.GetString("AuthToken");

                    if (string.IsNullOrEmpty(token))
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    var jsonContent = JsonConvert.SerializeObject(token);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync("http://web/api/Usuarios/logout", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Elimina el token de la sesión si el backend respondió correctamente
                        HttpContext.Session.Remove("AuthToken");
                        TempData["SucessMessage"] = "Contraseña cambiada con éxito. Vuelva a iniciar sesión";
                        return RedirectToAction("Login", "Account");
                    }

                }

                var errorMessage = await responseChangpwd.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            return View(model);
        }


        // GET: Account/Login
        [HttpGet]
        [ServiceFilter(typeof(RedirectIfAuthenticatedFilter))]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://web/api/Usuarios/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<LoginResponse>(responseBody)?.Token;

                    if (!string.IsNullOrEmpty(token))
                    {
                        HttpContext.Session.SetString("AuthToken", token);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No se pudo obtener el token de autenticación.");
                    }
                } 
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Session.GetString("AuthToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var jsonContent = JsonConvert.SerializeObject(token);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://web/api/Usuarios/logout", content);

            if (response.IsSuccessStatusCode)
            {
                // Elimina el token de la sesión si el backend respondió correctamente
                HttpContext.Session.Remove("AuthToken");
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // En caso de error, mostrar un mensaje
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "Error al cerrar sesión. Intentalo nuevamente");
                return View("Index", "Home");
            }
        }

    }
}
