using Ordering.Infrastructure;

namespace Infrastructure
{
    public record OrderCreated(Guid Id, string CustomerName, string PhoneNumber, string Address, string DeliveryTime, List<BasketItemDto> Items, DateTimeOffset CreatedDate);
    public record OrderUpdated(Guid Id, string CustomerName, string PhoneNumber, string Address, string DeliveryTime, List<BasketItemDto> Items, DateTimeOffset CreatedDate);
    public record OrderPrepared(Guid Id, string CustomerName, string PhoneNumber, string Address, string DeliveryTime, List<BasketItemDto> Items, string Status, DateTimeOffset CreatedDate);
    public record OrderDeleted(Guid Id);

    public record BasketCheckoutCompleted(Guid CustomerId, string CustomerName, decimal TotalPrice, string Address, string PhoneNumber, string DeliveryTime, List<BasketItemDto> Items, DateTimeOffset CreatedDate);
    public record OrderDeliveryCompleted(Guid OrderId);
}
