using System.ComponentModel.DataAnnotations;

namespace WebClient.Models
{
    public class BasketCheckoutModel
    {
        public decimal TotalPrice { get; set; } = 0;

        [Required (ErrorMessage = "Обязательное поле")]
        public string CustomerName { get; set; } = string.Empty;
        [Required (ErrorMessage = "Обязательное поле")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required (ErrorMessage = "Обязательное поле")]
        public string Address { get; set; } = string.Empty;
        [Required (ErrorMessage = "Обязательное поле")]
        public string DeliveryTime { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;

        [Required (ErrorMessage = "Обязательное поле")]
        public string CardName { get; set; } = string.Empty;
        [Required (ErrorMessage = "Обязательное поле")]
        public string CardNumber { get; set; } = string.Empty;
        [Required (ErrorMessage = "Обязательное поле")]
        public string Expiration { get; set; } = string.Empty;
        [Required (ErrorMessage = "Обязательное поле")]
        public string Cvv { get; set; } = string.Empty;
    }
}
