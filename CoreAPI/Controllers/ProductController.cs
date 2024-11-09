using Microsoft.AspNetCore.Mvc;
using CoreAPI.Context;
using CoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Products/{id}
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);  // JSON formatında ürün verisini döndür
        }

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product data is required.");
            }

            // Ürünü ekle
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Yeni eklenen ürünün URI'sini döndür
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
    }
}
