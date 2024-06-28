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
        public string CardHolderName { get; set; } = string.Empty;
        [Required (ErrorMessage = "Обязательное поле")]
        public string CardNumber { get; set; } = string.Empty;
        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "Неправильный формат срока действия карты!")]
        public string Expiration { get; set; } = string.Empty;
        [Required (ErrorMessage = "Обязательное поле")]
        public string Cvv { get; set; } = string.Empty;
    }
}
