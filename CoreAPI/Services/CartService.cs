using CoreAPI.Context;
using CoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get the current user's cart
        public async Task<Cart> GetCartByUserIdAsync(string userId)
        {
            var cart = await _context.Carts
                                     .Include(c => c.CartItems)
                                     .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                // If the cart doesn't exist, create a new one
                cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        // Add a product to the cart
        public async Task AddToCartAsync(string userId, int productId, int quantity)
        {
            var cart = await GetCartByUserIdAsync(userId);

            // Check if the item already exists in the cart
            var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (existingCartItem != null)
            {
                // If it exists, update the quantity
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // If it's a new item, create a new cart item
                var newCartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                };
                cart.CartItems.Add(newCartItem);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        // Remove an item from the cart
        public async Task RemoveFromCartAsync(string userId, int cartItemId)
        {
            var cart = await GetCartByUserIdAsync(userId);

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);

            if (cartItem != null)
            {
                cart.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        // Update the quantity of an item in the cart
        public async Task UpdateCartItemQuantityAsync(string userId, int cartItemId, int quantity)
        {
            var cart = await GetCartByUserIdAsync(userId);

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
        }

        // Clear the entire cart
        public async Task ClearCartAsync(string userId)
        {
            var cart = await GetCartByUserIdAsync(userId);

            cart.CartItems.Clear();
            await _context.SaveChangesAsync();
        }
    }
}
