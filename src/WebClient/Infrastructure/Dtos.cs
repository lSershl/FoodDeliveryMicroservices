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
        [Required] string CardName,
        [Required] string CardNumber,
        [Required] string Expiration,
        [Required] string Cvv,
        List<BasketItem> Items,
        DateTimeOffset CreatedDate);
}
