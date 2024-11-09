using Microsoft.EntityFrameworkCore;
using CoreAPI.Context;
namespace CoreAPI.Models
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Sepet oluşturma
        public async Task<Cart> CreateCartAsync(string cartSessionId)
        {
            var cart = new Cart(cartSessionId)
            {
                CartSessionId = cartSessionId,
                CartItems = new List<CartItem>()
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

        // Sepete ürün ekleme
        public async Task AddProductToCartAsync(string cartSessionId, int productId, int quantity)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.CartSessionId == cartSessionId);
            var product = await _context.Products.FindAsync(productId);

            if (cart == null || product == null) return;

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity; // Aynı üründen daha fazla eklenirse, miktarı artır
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

        // Sepetteki ürünleri görüntüleme
        public async Task<Cart> GetCartAsync(string cartSessionId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.CartSessionId == cartSessionId);
        }

        // Sepetten ürün silme
        public async Task RemoveProductFromCartAsync(string cartSessionId, int productId)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.CartSessionId == cartSessionId);
            var cartItem = cart?.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem != null)
            {
                cart.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
