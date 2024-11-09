using CoreAPI.Models;
using CoreAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly IUserService _userService; // Add IUserService for fetching the current user ID

        public CartController(CartService cartService, IUserService userService)
        {
            _cartService = cartService;
            _userService = userService;
        }

        // Sepet oluşturma (Yeni bir CartSessionId ile)
        [HttpPost("create")]
        public async Task<IActionResult> CreateCart()
        {
            var cartSessionId = Guid.NewGuid().ToString(); // Benzersiz bir sepet ID'si oluşturuluyor
            await _cartService.CreateCartAsync(cartSessionId);  // Just perform the action, don't assign it to a variable
            return Ok(new { CartSessionId = cartSessionId }); // Sepet ID'si döndürülüyor
        }

        // Sepete ürün ekleme
        [HttpPost("add/{cartSessionId}")]
        public async Task<IActionResult> AddProductToCart(string cartSessionId, [FromBody] AddProductToCartRequest request)
        {
            if (request == null || request.ProductId <= 0 || request.Quantity <= 0)
            {
                return BadRequest("Invalid product details.");
            }

            try
            {
                await _cartService.AddProductToCartAsync(cartSessionId, request.ProductId, request.Quantity);
                return Ok("Product added to cart.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Sepetteki ürünleri görme
        [HttpGet("{cartSessionId}")]
        public async Task<IActionResult> GetCart(string cartSessionId)
        {
            if (string.IsNullOrEmpty(cartSessionId))
            {
                return BadRequest("Cart session ID is required.");
            }

            var cart = await _cartService.GetCartAsync(cartSessionId);
            if (cart == null)
            {
                return NotFound("Cart not found.");
            }

            return Ok(cart);
        }

        // Sepetten ürün silme
        [HttpDelete("remove/{cartSessionId}/{productId}")]
        public async Task<IActionResult> RemoveProductFromCart(string cartSessionId, int productId)
        {
            if (string.IsNullOrEmpty(cartSessionId) || productId <= 0)
            {
                return BadRequest("Invalid cart session ID or product ID.");
            }

            try
            {
                await _cartService.RemoveProductFromCartAsync(cartSessionId, productId);
                return Ok("Product removed from cart.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
