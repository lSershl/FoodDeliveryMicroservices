using Basket.Entities;
using Basket.Infrastructure;

namespace Infrastructure
{
    public record BasketCheckoutCompleted(string CustomerName, decimal TotalPrice, string Address, string PhoneNumber, string DeliveryTime, List<BasketItemDto> Items, DateTimeOffset CreatedDate);
}
