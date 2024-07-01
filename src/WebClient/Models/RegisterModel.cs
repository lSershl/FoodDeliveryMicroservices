using System.ComponentModel.DataAnnotations;

namespace WebClient.Models
{
    public class RegisterModel
    {
        //[RegularExpression(@"^((8|\\+7)[\\- ]?)?(\\(?\\d{3}\\)?[\\- ]?)?[\\d\\- ]{7,10}$", ErrorMessage = "Неправильный номер телефона!")]
        [Phone(ErrorMessage = "Неправильный номер телефона!")]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Пароль должен быть не короче 6 символов!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Имя не должно быть пустым!")]
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; } = DateTime.Today;

        public string Email { get; set; } = string.Empty;
    }
}
