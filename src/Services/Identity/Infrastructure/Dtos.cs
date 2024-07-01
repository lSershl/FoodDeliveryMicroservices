using Identity.Entities;
using System.ComponentModel.DataAnnotations;

namespace Identity.Infrastructure
{
    public record LoginDto([Required] string PhoneNumber, [Required] string Password);

    public record RegisterDto(
        [Required] string PhoneNumber,
        [Required] string Password,
        [Required] string Name,
        DateTime Birthday,
        string? Email
        );

    public record SavedAddressDto(string Address);

    public record SavedPaymentCardDto(string PartialCardNumber);

    public record NewAddressDto (
        [Required] Guid CustomerId,
        [Required] string City,
        [Required] string Street,
        [Required] string House,
        [Required] string Apartment
        );

    public record NewPaymentCardDto (
        [Required] Guid CustomerId,
        [Required] string CardNumber,
        [Required] string CardHolderName,
        [Required] string Expiration,
        [Required] string Cvv
        );
}
