using Basket.Entities;
using System.ComponentModel.DataAnnotations;

namespace Basket.Infrastructure
{
    public record BasketItemDto(Guid Id, Guid ProductId, string ProductName,[Range(0, 10000)] decimal Price,[Range(0, 100)] int Quantity, string PictureUrl);
    public record CustomerBasketDto(Guid CustomerId, List<BasketItem> Items);
    public record GrantBasketDto(List<BasketItem> Items);
}
