using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreAPI.Context;
using CoreAPI.Models;
using System.IO;
using System.Threading.Tasks;
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
        public async Task<IActionResult> PostProduct([FromForm] ProductRequest productRequest, IFormFile image)
        {
            if (productRequest == null || productRequest.Product == null || image == null)
            {
                return BadRequest("Invalid product data or image.");
            }

            // Resim yükleme işlemi
            var imagePath = await UploadImageAsync(image);  // Dosya yükleme işlemi

            if (string.IsNullOrEmpty(imagePath))
            {
                return BadRequest("Resim yükleme işlemi başarısız oldu.");
            }

            // Ürünü al
            var product = productRequest.Product;

            // Resmin yolunu ve diğer bilgileri ürün objesine ekle
            product.ImageUrl = imagePath;
            product.ImageFileName = Path.GetFileName(image.FileName);  // Dosya adı
            product.ImageContentType = image.ContentType;  // İçerik tipi
            product.ImageData = await GetImageDataAsync(image);  // Resmin bayt verisi

            // Ürünü veritabanına kaydet
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Yeni eklenen ürünün URI'sini döndür
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // Resim yükleme işlemi (dosyayı kaydetme)
        private async Task<string?> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            // Dosyanın uzantısını kontrol et
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (fileExtension != ".jpg" && fileExtension != ".jpeg")
            {
                return null;  // Sadece .jpg veya .jpeg dosyalarını kabul et
            }

            // Dosya adı oluştur
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine("wwwroot/images", fileName);  // wwwroot/images klasörüne kaydedilir.

            // Dosyayı kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;  // Resmin yolu döndür
        }

        // Resmin bayt verisini almak için yardımcı metot
        private async Task<byte[]> GetImageDataAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }

    // Ürün verilerini ve dosya yükleme için yardımcı model
    public class ProductRequest
    {
        public required Product Product { get; set; }  // Ürün detayları
    }
}
