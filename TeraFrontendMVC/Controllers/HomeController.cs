using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Net.Http;
using TeraFrontendMVC.Models;
using TeraFrontendMVC.Models.Results;

namespace TeraFrontendMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(HttpClient httpClient, ILogger<HomeController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            string url = $"http://web/api/Resultados/resultados";
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();

                    // Deserializar la respuesta JSON en el modelo ResultadosResponse
                    var resultadosResponse = JsonConvert.DeserializeObject<ResultsResponseViewModel>(jsonData);

                    if (resultadosResponse != null)
                    {
                        return View(resultadosResponse);
                    }
                }
            }

                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
