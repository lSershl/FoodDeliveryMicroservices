using System.ComponentModel.DataAnnotations;

namespace Identity.Infrastructure
{
    public record LoginDto(string PhoneNumber, string Password);
    public record RegisterDto(
        [Required] string PhoneNumber,
        [Required] string Password,
        [Required] string Name,
        [Required] string Address,
        [Required] DateTime Birthday,
        string? Email
        );
}
