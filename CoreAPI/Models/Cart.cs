using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
namespace CoreAPI.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string CartSessionId { get; set; } // Anonim kullanıcılar için sepetin benzersiz kimliği
        public ICollection<CartItem> CartItems { get; set; }
        public decimal TotalPrice => CartItems?.Sum(x => x.Quantity * x.Product.Price) ?? 0; // Sepetteki toplam fiyat

    }
}
