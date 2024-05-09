using static Delivery.Infrastructure.Dtos;

namespace Infrastructure
{
    public record OrderCreated(Guid Id, Guid CustomerId, string CustomerName, string PhoneNumber, string Address, string DeliveryTime, List<BasketItemDto> Items, string Status, DateTimeOffset CreatedDate);
    public record OrderUpdated(Guid Id, Guid CustomerId, string CustomerName, string PhoneNumber, string Address, string DeliveryTime, List<BasketItemDto> Items, string Status, DateTimeOffset CreatedDate);
    public record OrderPrepared(Guid Id, string CustomerName, string PhoneNumber, string Address, string DeliveryTime, List<BasketItemDto> Items, string Status, DateTimeOffset CreatedDate);
    public record OrderDeleted(Guid Id);

    public record OrderDeliveryCompleted(Guid OrderId);
}
