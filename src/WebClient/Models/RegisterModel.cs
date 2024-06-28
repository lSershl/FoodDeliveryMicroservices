using System.ComponentModel.DataAnnotations;

namespace WebClient.Models
{
    public class RegisterModel
    {
        [RegularExpression(@"^((8|\\+7)[\\- ]?)?(\\(?\\d{3}\\)?[\\- ]?)?[\\d\\- ]{7,10}$", ErrorMessage = "Неправильный номер телефона!")]
        public required string PhoneNumber { get; set; }

        [StringLength(100, ErrorMessage = "Пароль должен быть не короче 6 символов!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        public required string ConfirmPassword { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        public required ApplicationUser User { get; set; }
    }
}
