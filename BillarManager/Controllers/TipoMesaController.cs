using BillarManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace BillarManager.Controllers
{
    public class TipoMesaController : Controller
    {
        private readonly IHttpClientFactory http;
        public TipoMesaController(IHttpClientFactory _http)
        { 
            http = _http;
        }
        JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        public async Task<IActionResult> Index()
        {
            var data = await GetTipoMesas();
            return View(data);
        }
        public async Task<List<TipoMesas>> GetTipoMesas()
        {
            var client = http.CreateClient("HerokuApi");
            var response = await client.GetAsync("tipomesa/");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<TipoMesas>>(content, options);
            return result;
        }
        [HttpPost]
        public async Task<IActionResult> NuevoTipoMesa(TipoMesas data)
        {
            var client = this.http.CreateClient("HerokuApi");
            var send = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("tipomesa/", send);
            var content = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EliminarTipoMesa(string id)
        {
            var client = this.http.CreateClient("HerokuApi");
            var response = await client.DeleteAsync($"tipomesa/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
          
        }
        [HttpGet]
        public async Task<TipoMesas> GetTipoMesa(int id)
        {
            var client = this.http.CreateClient("HerokuApi");
            var response = await client.GetAsync($"tipomesa/{id}");
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<TipoMesas>(content,options);
            return json;
        }

        [HttpPost]
        public async Task<IActionResult> EditarTipoMesa(TipoMesas data)
        {
            var client = this.http.CreateClient("HerokuApi");
            var send = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"tipomesa/{data.idTipo}/", send);
            var content = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
        }
        
    }
}
