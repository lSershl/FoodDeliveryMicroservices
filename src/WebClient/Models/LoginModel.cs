using System.ComponentModel.DataAnnotations;

namespace WebClient.Models
{
    public class LoginModel
    {
        //[RegularExpression(@"^((8|\\+7)[\\- ]?)?(\\(?\\d{3}\\)?[\\- ]?)?[\\d\\- ]{7,10}$", ErrorMessage = "Неправильный номер телефона!")]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
