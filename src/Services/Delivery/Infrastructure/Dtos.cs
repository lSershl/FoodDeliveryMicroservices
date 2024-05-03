using System.ComponentModel.DataAnnotations;

namespace Delivery.Infrastructure
{
    public class Dtos
    {
        public record GrantOrderDto(Guid CourierId, Guid OrderId, string Status);
        public record DeliveryDto(Guid OrderId, string Address, string DeliveryTime, string Status, Guid DeliveryId, Guid CourierId, DateTimeOffset AcquiredDate);
        public record BasketItemDto(Guid ProductId, string ProductName, [Range(0, 10000)] decimal Price, [Range(0, 100)] int Quantity, string PictureUrl);
        public record OrderDto(Guid Id, string CustomerName, string PhoneNumber, string Address, string DeliveryTime, List<BasketItemDto> Items, string Status, DateTimeOffset CreatedDate);
    }
}
