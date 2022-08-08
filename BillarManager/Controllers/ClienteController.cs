using BillarManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace BillarManager.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IHttpClientFactory http;
        public ClienteController(IHttpClientFactory _http)
        { 
            http = _http;
        }
        JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        public async Task<IActionResult> Index()
        {
            var data = await GetClientes();
            return View(data);
        }
        public async Task<List<Cliente>> GetClientes()
        {
            var client = http.CreateClient("HerokuApi");
            var response = await client.GetAsync("cliente/");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<Cliente>>(content, options);
            return result;
        }
        [HttpGet]
        public IActionResult NuevoCliente()
        {
            return View("AgregarCliente");
        }
        [HttpPost]
        public async Task<IActionResult> NuevoCliente(Cliente data)
        {
            data.FechaRegistro = DateTime.Now.ToString("yyyy-MM-dd");
            var client = this.http.CreateClient("HerokuApi");
            var send = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("cliente/", send);
            var content = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Eliminar(string id)
        {
            var client = this.http.CreateClient("HerokuApi");
            var response = await client.DeleteAsync($"cliente/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
          
        }
        [HttpGet]
        public async Task<Cliente> GetCliente(int id)
        {
            var client = this.http.CreateClient("HerokuApi");
            var response = await client.GetAsync($"cliente/{id}");
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<Cliente>(content,options);
            return json;
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Cliente data)
        {
            var client = this.http.CreateClient("HerokuApi");
            var send = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"cliente/{data.Documento}/", send);
            var content = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index");
        }
        
    }
}
