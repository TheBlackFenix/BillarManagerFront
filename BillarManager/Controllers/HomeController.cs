using BillarManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace BillarManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory http;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory http)
        {
            _logger = logger;
            this.http = http;
        }
        JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Login(LoginData data)
        {
            var client = this.http.CreateClient("HerokuApi");
            var send = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("login/",send);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
             //   var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Token>(content, options);
                var mesas = await GetMesas();
                return View("Admin",mesas);
            }
            else {
                return View("Index");
            }
        }
        public async Task<List<Mesa>> GetMesas()
        {
            var client = this.http.CreateClient("HerokuApi");
            var response = await client.GetAsync("mesa/");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<Mesa>>(content, options);
            return result;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}