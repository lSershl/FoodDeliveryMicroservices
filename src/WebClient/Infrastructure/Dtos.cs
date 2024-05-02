using System.ComponentModel.DataAnnotations;
using WebClient.Models;

namespace WebClient.Infrastructure
{
    public record CatalogItemDto(Guid Id, string Name, string Description, string ImageUrl, decimal Price);
    public record BasketItemDto(Guid ProductId, string Name, decimal Price, [Range(0, 100)] int Quantity, string ImageUrl);
    public record CustomerBasketDto(Guid CustomerId, List<BasketItem> Items);
}
