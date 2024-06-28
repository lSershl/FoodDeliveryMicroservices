using Basket.Entities;
using System.ComponentModel.DataAnnotations;

namespace Basket.Infrastructure
{
    public record BasketItemDto(Guid ProductId, string ProductName,[Range(0, 10000)] decimal Price,[Range(0, 100)] int Quantity, string PictureUrl);
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
}
