using BillarManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace BillarManager.Controllers
{
    public class EstadoController : Controller
    {
        private readonly IHttpClientFactory http;
        public EstadoController(IHttpClientFactory _http)
        { 
            http = _http;
        }
        JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                var data = await GetEstados();
                return View(data);
            }
        }
        public async Task<List<Estados>> GetEstados()
        {
            var client = http.CreateClient("HerokuApi");
            var response = await client.GetAsync("estado/");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<Estados>>(content, options);
            return result;
        }
        [HttpPost]
        public async Task<IActionResult> NuevoEstado(Estados data)
        {
            var client = this.http.CreateClient("HerokuApi");
            var send = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("estado/", send);
            var content = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EliminarEstado(string id)
        {
            var client = this.http.CreateClient("HerokuApi");
            var response = await client.DeleteAsync($"estado/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
          
        }
        [HttpGet]
        public async Task<Estados> GetEstado(int id)
        {
            var client = this.http.CreateClient("HerokuApi");
            var response = await client.GetAsync($"estado/{id}");
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<Estados>(content,options);
            return json;
        }

        [HttpPost]
        public async Task<IActionResult> EditarEstado(Estados data)
        {
            var client = this.http.CreateClient("HerokuApi");
            var send = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"estado/{data.idEstado}/", send);
            var content = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
        }
        
    }
}
