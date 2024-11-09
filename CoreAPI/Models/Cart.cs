using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
namespace CoreAPI.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string CartSessionId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<CartItem> CartItems { get; set; }

        // Constructor with CartSessionId
        public Cart(string cartSessionId)
        {
            CartSessionId = cartSessionId;
            CartItems = new List<CartItem>(); // Initialize the collection
        }

        // Default constructor
        public Cart()
        {
            CartItems = new List<CartItem>(); // Initialize the collection
        }
    }

}
