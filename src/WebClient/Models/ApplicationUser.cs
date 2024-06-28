using System.ComponentModel.DataAnnotations;

namespace WebClient.Models
{
    public class ApplicationUser
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime Birthday { get; set; } = DateTime.Today;
        public string CardNumber { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "Неправильный формат срока действия карты!")]
        public string Expiration { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
    }
}
