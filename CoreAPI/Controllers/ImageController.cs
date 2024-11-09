using CoreAPI.Context;
using Microsoft.AspNetCore.Mvc;
using CoreAPI.Models;
using CoreAPI.Services;

namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly ImageService _imageService;

        public ImageController(ImageService imageService)
        {
            _imageService = imageService;
        }

        // Resim yükleme endpoint'i
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            await _imageService.UploadImageAsync(file);
            return Ok("Image uploaded successfully.");
        }

        // Resim çekme endpoint'i
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var image = await _imageService.GetImageByIdAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            return File(image.Data, image.ContentType); // Dosya verisini ve MIME tipini döndürüyoruz.
        }
    }
}
