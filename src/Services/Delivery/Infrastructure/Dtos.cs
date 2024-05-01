namespace Delivery.Infrastructure
{
    public class Dtos
    {
        public record GrantOrderDto(Guid CourierId, Guid OrderId, string Status);

        public record DeliveryDto(Guid OrderId, string Address, int Quantity, string Status, Guid DeliveryId, Guid CourierId, DateTimeOffset AcquiredDate);

        public record OrderDto(Guid Id, string Address, int Quantity);
    }
}
