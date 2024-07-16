using System.ComponentModel.DataAnnotations;
using WebClient.Models;

namespace WebClient.Infrastructure
{
    public record CatalogItemDto(Guid Id, string Name, string Description, string PictureUrl, decimal Price);
    public record OrderDto(Guid Id, string CustomerName, string PhoneNumber, string Address, string DeliveryTime, List<BasketItemDto> Items, string Status, DateTimeOffset CreatedDate);
    public record BasketItemDto(Guid ProductId, string ProductName, decimal Price, [Range(0, 100)] int Quantity, string PictureUrl);
    public record CustomerBasketDto(Guid CustomerId, List<BasketItem> Items);
    public record BasketCheckoutDto(
        decimal TotalPrice,
        [Required] string CustomerName,
        [Required] string PhoneNumber,
        [Required] string Address,
        [Required] string DeliveryTime, 
        string? Email,
        [Required] string CardHolderName,
        [Required] string CardNumber,
        [Required] string Expiration,
        [Required] string Cvv,
        List<BasketItem> Items,
        DateTimeOffset CreatedDate);
    public record SavedAddressDto(Guid Id, string FullAddress);
    public record NewAddressDto(
        [Required] Guid CustomerId,
        [Required] string City,
        [Required] string Street,
        [Required] string House,
        [Required] string Apartment
        );
    public record SavedPaymentCardDto(Guid Id, string PartialCardNumber);
    public record NewPaymentCardDto(
        [Required] Guid CustomerId,
        [Required] string CardNumber,
        [Required] string CardHolderName,
        [Required] string Expiration,
        [Required] string Cvv
        );
    public record LoginDto(string PhoneNumber, string Password);

    public record RegisterDto(
        [Required] string PhoneNumber,
        [Required] string Password,
        [Required] string Name,
        DateTime Birthday,
        string? Email
        );
    public record CustomUserClaims(string CustomerId = null!, string Name = null!, string PhoneNumber = null!, string Birthday = null!);
}
