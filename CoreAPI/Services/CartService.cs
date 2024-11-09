using Microsoft.EntityFrameworkCore;
using CoreAPI.Context;
using CoreAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        // Injecting the ApplicationDbContext and IUserService to access user-related logic
        public CartService(ApplicationDbContext context, IUserService userService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        // Create a cart for the current user
        public async Task CreateCartAsync(string cartSessionId)
        {
            if (string.IsNullOrEmpty(cartSessionId))
            {
                throw new ArgumentException("Cart session ID is required.", nameof(cartSessionId));
            }

            var userId = _userService.GetCurrentUserId();  // Get the current user ID
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User ID is required to create a cart.");
            }

            var cart = new Cart
            {
                UserId = userId,
                CartSessionId = cartSessionId,
                CreatedDate = DateTime.UtcNow
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        // Add a product to the cart
        public async Task AddProductToCartAsync(string cartSessionId, int productId, int quantity)
        {
            if (string.IsNullOrEmpty(cartSessionId))
            {
                throw new ArgumentException("Cart session ID is required.", nameof(cartSessionId));
            }

            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            }

            var cart = await _context.Carts
                                      .Include(c => c.CartItems)
                                      .FirstOrDefaultAsync(c => c.CartSessionId == cartSessionId);
            if (cart == null)
            {
                throw new Exception("Cart not found.");
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity; // If the product already exists, increase the quantity
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Product = product
                });
            }

            await _context.SaveChangesAsync();
        }

        // Get the cart with its items
        public async Task<Cart> GetCartAsync(string cartSessionId)
        {
            if (string.IsNullOrEmpty(cartSessionId))
            {
                throw new ArgumentException("Cart session ID is required.", nameof(cartSessionId));
            }

            var cart = await _context.Carts
                                      .Include(c => c.CartItems)
                                      .ThenInclude(ci => ci.Product)
                                      .FirstOrDefaultAsync(c => c.CartSessionId == cartSessionId);

            if (cart == null)
            {
                throw new Exception("Cart not found.");
            }

            return cart;
        }

        // Remove a product from the cart
        public async Task RemoveProductFromCartAsync(string cartSessionId, int productId)
        {
            if (string.IsNullOrEmpty(cartSessionId))
            {
                throw new ArgumentException("Cart session ID is required.", nameof(cartSessionId));
            }

            var cart = await _context.Carts
                                      .Include(c => c.CartItems)
                                      .FirstOrDefaultAsync(c => c.CartSessionId == cartSessionId);

            if (cart == null)
            {
                throw new Exception("Cart not found.");
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                cart.CartItems.Remove(cartItem);  // Remove the item from the cart
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Product not found in the cart.");
            }
        }
    }
}
