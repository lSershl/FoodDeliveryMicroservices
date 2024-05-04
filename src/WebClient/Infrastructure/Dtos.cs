using System.ComponentModel.DataAnnotations;
using WebClient.Models;

namespace WebClient.Infrastructure
{
    public record CatalogItemDto(Guid Id, string Name, string Description, string ImageUrl, decimal Price);
    public record BasketItemDto(Guid ProductId, string Name, decimal Price, [Range(0, 100)] int Quantity, string ImageUrl);
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
