namespace CoreAPI.Models
{
    public class Product
    {
        public int Id { get; set; } // Ürün kimliği
        public string? Name { get; set; } // Ürün adı
        public string? Description { get; set; } // Ürün açıklaması
        public decimal Price { get; set; } // Ürün fiyatı
        public int Stock { get; set; } // Ürün stoğu

        // Resim bilgileri
        public string? ImageUrl { get; set; }  // Yolu
        public string? ImageFileName { get; set; }  // Dosya adı
        public string? ImageContentType { get; set; }  // İçerik tipi
        public byte[]? ImageData { get; set; }  // Resmin bayt verisi

    }
}
