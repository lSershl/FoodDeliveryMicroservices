namespace WebClient.Models
{
    public class CustomerBasket
    {
        public Guid CustomerId { get; set; }
        public List<BasketItem>? Items { get; set; } = new List<BasketItem>();
    }
}
