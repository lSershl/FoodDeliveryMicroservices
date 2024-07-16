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

    public record SavedAddressDto(Guid Id, string FullAddress);

    public record SavedPaymentCardDto(Guid Id, string PartialCardNumber);

    public record PaymentCardInfoDto(string CardNumber, string CardHolderName, string Expiration, string Cvv);

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
