namespace WebClient.Models
{
    public class BasketCheckout
    {
        public decimal TotalPrice { get; set; }

        public required string CustomerName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
        public required string DeliveryTime { get; set; }
        public string? Email { get; set; }

        public required string CardName { get; set; }
        public required string CardNumber { get; set; }
        public required string Expiration { get; set; }
        public required string Cvv { get; set; }
    }
}
