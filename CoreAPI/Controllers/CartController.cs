using CoreAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        // Sepet oluşturma (Yeni bir CartSessionId ile)
        [HttpPost("create")]
        public async Task<IActionResult> CreateCart()
        {
            var cartSessionId = Guid.NewGuid().ToString(); // Benzersiz bir sepet ID'si oluşturuluyor
            var cart = await _cartService.CreateCartAsync(cartSessionId);
            return Ok(new { CartSessionId = cartSessionId }); // Sepet ID'si döndürülüyor
        }

        // Sepete ürün ekleme
        [HttpPost("add/{cartSessionId}")]
        public async Task<IActionResult> AddProductToCart(string cartSessionId, [FromBody] AddProductToCartRequest request)
        {
            await _cartService.AddProductToCartAsync(cartSessionId, request.ProductId, request.Quantity);
            return Ok();
        }

        // Sepetteki ürünleri görme
        [HttpGet("{cartSessionId}")]
        public async Task<IActionResult> GetCart(string cartSessionId)
        {
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
            await _cartService.RemoveProductFromCartAsync(cartSessionId, productId);
            return Ok();
        }


    }

}
