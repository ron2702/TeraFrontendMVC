using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeraFrontendMVC.Models;

namespace TeraFrontendMVC.Controllers
{
    public class Users : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<Users> _logger;

        public Users(HttpClient httpClient, ILogger<Users> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            List<UserViewModel> users = new List<UserViewModel>();

            try
            {
                var response = await _httpClient.GetAsync("http://web/api/Usuarios/listar-usuarios");

                if (response.IsSuccessStatusCode)
                {
                    users = await response.Content.ReadFromJsonAsync<List<UserViewModel>>();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                // Puedes registrar el error si es necesario
                Console.WriteLine(ex.Message);
            }

            return View(users);
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
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

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Users/Edit/5
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

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Users/Delete/5
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
