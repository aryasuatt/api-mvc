namespace CoreMVC.Models
{
    public class ToBuy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public float Price { get; set; }
        
        public int Month { get; set; }
        public bool IsBought { get; set; }
    }
}
