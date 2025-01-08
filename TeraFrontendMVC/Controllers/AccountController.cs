using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.Text;
using TeraFrontendMVC.Models;
using TeraFrontendMVC.Models.Account;

namespace TeraFrontendMVC.Controllers
{
    public class AccountController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<AccountController> _logger;
        private readonly ApiUrls _apiUrls;

        public AccountController(HttpClient httpClient, ILogger<AccountController> logger, IOptions<ApiUrls> apiUrls)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiUrls = apiUrls.Value;
        }

        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // GET: Account/ChangePassword
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {

            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            string url = $"{_apiUrls.Usuarios}/usuario-por-token";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var userProfile = JsonConvert.DeserializeObject<UserProfile>(jsonResponse);
                var changePasswordModel = new Models.Account.ChangePassword
                {
                    Email = userProfile.Email
                };
                return View(changePasswordModel);
            }

            // Si hay un error, muestra un mensaje y redirige
            TempData["ErrorMessage"] = "Error al obtener los datos del perfil de usuario.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            string url = $"{_apiUrls.Usuarios}/usuario-por-token";

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var userProfile = JsonConvert.DeserializeObject<UserProfile>(jsonResponse);

                return View(userProfile);
            }

            // Si hay un error, muestra un mensaje y redirige
            TempData["ErrorMessage"] = "Error al obtener los datos del perfil de usuario.";
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/ListUsersForAdmin
        [HttpGet]
        public async Task<IActionResult> ListUsersForAdmin(int page = 1, int pageSize = 10)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "Inicia sesión como administrador para acceder a esta funcionalidad.";
                return RedirectToAction("Login", "Account");
            }
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var url = $"{_apiUrls.Usuarios}/listar-usuarios-admin?page={page}&pageSize={pageSize}";

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var userListResponse = JsonConvert.DeserializeObject<UserListResponse>(jsonResponse);

                    return View(userListResponse);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Error al listar usuarios: {errorMessage}";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de usuarios.");
                TempData["ErrorMessage"] = "Ocurrió un error al procesar la solicitud.";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Account/EditUser
        [HttpGet]
        public async Task<IActionResult> EditUser(string email = null)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


            string url = string.IsNullOrEmpty(email) ? $"{_apiUrls.Usuarios}/usuario-por-token" : $"{_apiUrls.Usuarios}/usuario-por-email?email={email}";

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var userProfile = JsonConvert.DeserializeObject<UpdateUser>(jsonResponse);

                return View(userProfile);
            }

            TempData["ErrorMessage"] = "Error al obtener los datos del usuario.";
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/RegisterForAdmin
        [HttpGet]
        public IActionResult RegisterForAdmin()
        {
            return View();
        }


        // POST: Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Role))
                {
                    model.Role = "Usuario"; // Rol por defecto
                }

                string url = $"{_apiUrls.Usuarios}/registrar-usuario";

                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

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

        [HttpPost]
        public async Task<IActionResult> RegisterForAdmin(Register model)
        {
            if (ModelState.IsValid)
            {
                var token = HttpContext.Session.GetString("AuthToken");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["ErrorMessage"] = "No se encontró un token de autenticación. Por favor, inicia sesión nuevamente.";
                    return RedirectToAction("Login", "Account");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                string url = $"{_apiUrls.Usuarios}/registrar-para-admin";

                try
                {
                    var response = await _httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Usuario registrado con éxito";
                        return RedirectToAction("UserProfile", "Account");
                    }

                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Error al registrar: {errorMessage}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al realizar la solicitud al backend.");
                    ModelState.AddModelError(string.Empty, "Ocurrió un error al comunicarse con el servidor. Inténtalo nuevamente.");
                }
            }

            return View(model);
        }

        // POST: Account/EditUser
        [HttpPost]
        public async Task<IActionResult> EditUser(UpdateUser model)
        {
            if (ModelState.IsValid)
            {
                var token = HttpContext.Session.GetString("AuthToken");
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Account");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                string url = $"{_apiUrls.Usuarios}/editar-usuario";

                var response = await _httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Usuario actualizado correctamente";
                    return RedirectToAction("UserProfile", "Account");
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Error al actualizar usuario: {errorMessage}");
            }

            return View(model);
        }

        // POST: Account/ChangePassword
        [HttpPost]
        public async Task<IActionResult> ChangePassword(Models.Account.ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var changpwdjson = JsonConvert.SerializeObject(model);
                var changepwdcontent = new StringContent(changpwdjson, Encoding.UTF8, "application/json");

                string url = $"{_apiUrls.Usuarios}/modificar-contrasena";

                var responseChangpwd = await _httpClient.PostAsync(url, changepwdcontent);

                if (responseChangpwd.IsSuccessStatusCode)
                {

                    var token = HttpContext.Session.GetString("AuthToken");

                    if (string.IsNullOrEmpty(token))
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    var jsonContent = JsonConvert.SerializeObject(token);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    string url2 = $"{_apiUrls.Usuarios}/logout";

                    var response = await _httpClient.PostAsync(url2, content);

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

        // POST: Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                string url = $"{_apiUrls.Usuarios}/login";

                var response = await _httpClient.PostAsync(url, content);

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

        // POST Logout
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

            string url = $"{_apiUrls.Usuarios}/logout";

            var response = await _httpClient.PostAsync(url, content);

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
