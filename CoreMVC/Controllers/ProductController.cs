using Microsoft.AspNetCore.Mvc;
using CoreMVC.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CoreAPI.Models;

namespace CoreMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Tek bir ürünü görüntüle
        public async Task<IActionResult> Details(int id)
        {
            // API'den ürün detaylarını al
            var product = await _httpClient.GetFromJsonAsync<Product>($"https://localhost:5001/api/products/{id}");

            if (product == null)
            {
                return NotFound(); // Ürün bulunamazsa 404 döndür
            }

            return View(product); // Ürün detayını göster
        }
    }
}
