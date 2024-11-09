namespace CoreAPI.Models
{
    public class AddProductToCartRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
