namespace CoreAPI.Models
{
    public class Product
    {
        public int Id { get; set; } // Ürün kimliği
        public string Name { get; set; } // Ürün adı
        public string Description { get; set; } // Ürün açıklaması
        public decimal Price { get; set; } // Ürün fiyatı
        public int Stock { get; set; } // Ürün stoğu
        public int ImageId { get; set; }
        public Image Image { get; set; }
        public DateTime? CreatedDate { get; set; } // Ürün oluşturulma tarihi
        public DateTime? UpdatedDate { get; set; } // Ürün güncellenme tarihi

    }
}
