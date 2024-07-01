using System.ComponentModel.DataAnnotations;

namespace WebClient.Models
{
    public class PaymentCardInfo
    {
        public string CardNumber { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "Неправильный формат срока действия карты!")]
        public string Expiration { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
    }
}
