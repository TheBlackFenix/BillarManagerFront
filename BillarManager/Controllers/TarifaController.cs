using BillarManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace BillarManager.Controllers
{
    public class TarifaController : Controller
    {
        private readonly IHttpClientFactory http;
        public TarifaController(IHttpClientFactory _http)
        { 
            http = _http;
        }
        JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        public async Task<IActionResult> Index()
        {
            var data = await GetTarifas();
            return View(data);
        }
        public async Task<List<Tarifas>> GetTarifas()
        {
            var client = http.CreateClient("HerokuApi");
            var response = await client.GetAsync("tarifa/");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<Tarifas>>(content, options);
            return result;
        }
        [HttpPost]
        public async Task<IActionResult> NuevoTarifa(Tarifas data)
        {
            var client = this.http.CreateClient("HerokuApi");
            var send = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("tarifa/", send);
            var content = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EliminarTarifa(string id)
        {
            var client = this.http.CreateClient("HerokuApi");
            var response = await client.DeleteAsync($"tarifa/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
          
        }
        [HttpGet]
        public async Task<Tarifas> GetTarifa(int id)
        {
            var client = this.http.CreateClient("HerokuApi");
            var response = await client.GetAsync($"tarifa/{id}");
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<Tarifas>(content,options);
            return json;
        }

        [HttpPost]
        public async Task<IActionResult> EditarTarifa(Tarifas data)
        {
            var client = this.http.CreateClient("HerokuApi");
            var send = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"tarifa/{data.idTarifa}/", send);
            var content = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
        }
        
    }
}
