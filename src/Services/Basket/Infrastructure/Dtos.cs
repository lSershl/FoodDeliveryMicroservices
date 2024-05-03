using Basket.Entities;
using System.ComponentModel.DataAnnotations;

namespace Basket.Infrastructure
{
    public record BasketItemDto(Guid ProductId, string ProductName,[Range(0, 10000)] decimal Price,[Range(0, 100)] int Quantity, string PictureUrl);
    public record CustomerBasketDto(Guid CustomerId, List<BasketItem> Items);
    public record BasketCheckoutDto(Guid CustomerId, decimal TotalPrice, string CustomerName, string PhoneNumber, string Address, string DeliveryTime, List<BasketItemDto> Items, string Status, DateTimeOffset CreatedDate);
}
