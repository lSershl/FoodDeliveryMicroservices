using System.ComponentModel.DataAnnotations;

namespace WebClient.Models
{
    public class CheckoutModel
    {
        public decimal TotalPrice { get; set; } = 0;
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DeliveryTime { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string Expiration { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
    }
}
