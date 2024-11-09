using Microsoft.EntityFrameworkCore;
using CoreAPI.Context;
using CoreAPI.Models;
namespace CoreAPI.Services
{
    public class ImageService
    {
        private readonly ApplicationDbContext _context;

        public ImageService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Resmi veritabanına yükleme
        public async Task UploadImageAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] imageData = memoryStream.ToArray();

                var image = new Image
                {
                    Data = imageData,
                    FileName = file.FileName,
                    ContentType = file.ContentType
                };

                _context.Images.Add(image);
                await _context.SaveChangesAsync();
            }
        }

        // Veritabanından resmi çekme
        public async Task<Image> GetImageByIdAsync(int id)
        {
            return await _context.Images.FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
