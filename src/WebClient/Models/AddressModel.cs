using System.ComponentModel.DataAnnotations;

namespace WebClient.Models
{
    public class AddressModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        public string City { get; set; } = string.Empty;
        [Required(ErrorMessage = "Обязательное поле")]
        public string Street { get; set; } = string.Empty;
        [Required(ErrorMessage = "Обязательное поле")]
        public string House { get; set; } = string.Empty;
        [Required(ErrorMessage = "Обязательное поле")]
        public string Apartment { get; set; } = string.Empty;
    }
}
