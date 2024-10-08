using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TeraFrontendMVC.Models;

namespace TeraFrontendMVC.Controllers
{
    public class ResultsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<Users> _logger;

        public ResultsController(HttpClient httpClient, ILogger<Users> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // GET: Results
        public async Task<IActionResult> Index()
        {
            // Obtener los estados, municipios y parroquias reutilizando los métodos privados
            var states = await GetStates(null); // null significa que no hay codEdo para filtrar
            var municipalities = await GetMunicipality(null, null); // No hay codMun ni codEdo
            var parishes = await GetParish(null); // No hay codPar

            // Crear un modelo para pasar a la vista
            var model = new SelectsViewModel
            {
                States = states,
                Municipalities = municipalities,
                Parishes = parishes
            };

            return View(model);
        }

        // GET: Results/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Results/Create
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

        // GET: Results/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Results/Edit/5
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

        // GET: Results/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Results/Delete/5
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

        private async Task<List<StateViewModel>> GetStates(int? codEdo)
        {

            List<StateViewModel> states = new List<StateViewModel>();

            try
            {
                // Crear la query string condicionalmente según los parámetros
                var queryString = $"http://web/api/Region/estados?";
                if (codEdo.HasValue)
                {
                    queryString += $"codEdo={codEdo.Value}";
                }

                // Eliminar el último '&' sobrante o '?'
                queryString = queryString.TrimEnd('&', '?');
                var response = await _httpClient.GetAsync(queryString);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    states = JsonSerializer.Deserialize<List<StateViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    _logger.LogError("Error al obtener la lista de estados. Status Code: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return states;
        }

        private async Task<List<MunicipalityViewModel>> GetMunicipality(int? codMun, int? codEdo)
        {

            List<MunicipalityViewModel> municipality = new List<MunicipalityViewModel>();

            try
            {
                // Crear la query string condicionalmente según los parámetros
                var queryString = $"http://web/api/Region/municipios?";
                if (codMun.HasValue)
                {
                    queryString += $"codMun={codMun.Value}&";
                }
                if (codEdo.HasValue)
                {
                    queryString += $"codEdo={codEdo.Value}&";
                }

                // Eliminar el último '&' sobrante o '?'
                queryString = queryString.TrimEnd('&', '?');

                // Realiza la solicitud al backend con los parámetros necesarios
                var response = await _httpClient.GetAsync(queryString);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    municipality = JsonSerializer.Deserialize<List<MunicipalityViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    _logger.LogError("Error al obtener la lista de municipios. Status Code: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return municipality;
        }

        private async Task<List<ParishViewModel>> GetParish(int? codPar)
        {

            List<ParishViewModel> parish = new List<ParishViewModel>();

            try
            {
                // Crear la query string condicionalmente según los parámetros
                var queryString = $"http://web/api/Region/parroquias?";
                if (codPar.HasValue)
                {
                    Console.WriteLine("Entre al if");
                    queryString += $"codPar={codPar.Value}";
                    Console.WriteLine(queryString);
                }

                // Eliminar el último '&' sobrante o '?'
                queryString = queryString.TrimEnd('&', '?');

                // Realiza la solicitud al backend con los parámetros necesarios
                var response = await _httpClient.GetAsync(queryString);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    parish = JsonSerializer.Deserialize<List<ParishViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    _logger.LogError("Error al obtener la lista de municipios. Status Code: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return parish;
        }
    }
}
