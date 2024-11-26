using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.Json;
using TeraFrontendMVC.Filter;
using TeraFrontendMVC.Models.Region;
using TeraFrontendMVC.Models.Results;

namespace TeraFrontendMVC.Controllers
{
    public class ResultsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ResultsController> _logger;

        public ResultsController(HttpClient httpClient, ILogger<ResultsController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // GET: Results
        [ServiceFilter(typeof(RedirectIfNotAuthenticatedFilter))]
        public async Task<IActionResult> Index()
        {
            // Obtener los estados, municipios y parroquias reutilizando los métodos privados
            var states = await GetStates(null);
            var municipalities = new List<MunicipalityViewModel>();
            var parishes = new List<ParishViewModel>();

            // Crear un modelo para pasar a la vista
            var model = new SelectsViewModel
            {
                States = states,
                Municipalities = municipalities,
                Parishes = parishes
            };

            return View(model);
        }

        [HttpGet]
        [ServiceFilter(typeof(RedirectIfNotAuthenticatedFilter))]
        public async Task<IActionResult> Buscar(int? codEdo, int? munId, int? codPar, int? pageSize, int? pageNumber)
        {

            try
            {
                var token = HttpContext.Session.GetString("AuthToken");

                string url = $"http://web/api/Resultados/resultados?codEdo={codEdo}&munId={munId}&codPar={codPar}&pageSize={pageSize}&pageNumber={pageNumber}";

                using (HttpClient client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();

                        // Deserializar la respuesta JSON en el modelo ResultadosResponse
                        var resultadosResponse = JsonConvert.DeserializeObject<ResultsResponseViewModel>(jsonData);

                        if (resultadosResponse != null)
                        {
                            resultadosResponse.PageNumber = pageNumber?.ToString() ?? "1";
                            return PartialView("_ResultsPartial", resultadosResponse);
                        }
                        else
                        {
                            _logger.LogError("La respuesta JSON es nula o no tiene el formato esperado.");
                            ViewBag.ErrorMessage = "Ocurrió un error al obtener los resultados. Por favor, verifica tu selección e intenta nuevamente.";
                            return PartialView("_ResultsPartial");
                        }
                    }
                    else
                    {
                        _logger.LogError("Error al buscar los resultados: " + response.StatusCode);
                        ViewBag.ErrorMessage = "Ocurrió un error al obtener los resultados. Por favor, verifica tu selección e intenta nuevamente.";
                        return PartialView("_ResultsPartial");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al buscar los resultados.");
                ViewBag.ErrorMessage = "Ocurrió un error al obtener los resultados. Por favor, verifica tu selección e intenta nuevamente.";
                return PartialView("_ResultsPartial");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMunicipios(int codEdo)
        {
            var municipios = await GetMunicipality(null, codEdo); // Obtener municipios filtrados por estado
            return Json(municipios);
        }

        [HttpGet]
        public async Task<IActionResult> GetParroquias(int codMun)
        {
            var parroquias = await GetParish(codMun, null); // Obtener parroquias filtradas por municipio
            return Json(parroquias);
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

                // Enviar authToken en el header
                var token = HttpContext.Session.GetString("AuthToken");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(queryString);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    states = System.Text.Json.JsonSerializer.Deserialize<List<StateViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    // Aplicar el formato al nombre de cada estado
                    states.ForEach(state => state.Nombre = FormatName(state.Nombre));
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

                // Enviar authToken en el header
                var token = HttpContext.Session.GetString("AuthToken");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Realiza la solicitud al backend con los parámetros necesarios
                var response = await _httpClient.GetAsync(queryString);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    municipality = System.Text.Json.JsonSerializer.Deserialize<List<MunicipalityViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    municipality.ForEach(mun => mun.Nombre = FormatName(mun.Nombre));
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

        private async Task<List<ParishViewModel>> GetParish(int? codMun, int? codPar)
        {

            List<ParishViewModel> parish = new List<ParishViewModel>();

            try
            {
                // Crear la query string condicionalmente según los parámetros
                var queryString = $"http://web/api/Region/parroquias?";
                if (codMun.HasValue)
                {
                    queryString += $"codMun={codMun.Value}";
                }
                if (codPar.HasValue)
                {
                    Console.WriteLine("Entre al if");
                    queryString += $"codPar={codPar.Value}";
                }

                // Eliminar el último '&' sobrante o '?'
                queryString = queryString.TrimEnd('&', '?');

                // Enviar authToken en el header
                var token = HttpContext.Session.GetString("AuthToken");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Realiza la solicitud al backend con los parámetros necesarios
                var response = await _httpClient.GetAsync(queryString);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    parish = System.Text.Json.JsonSerializer.Deserialize<List<ParishViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    parish.ForEach(par => par.Nombre = FormatName(par.Nombre));
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

        private string FormatName(string name)
        {
            string sanitizedInput = name.Replace("\uFFFD", "ñ");
            sanitizedInput = sanitizedInput.ToLower();
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(sanitizedInput);
        }
    }
}
