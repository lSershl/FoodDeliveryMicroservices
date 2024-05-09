using Ordering.Entities;

namespace Ordering.Infrastructure
{
    public static class Extensions
    {
        public static OrderDto AsDto(this Order order)
        {
            return new OrderDto(order.Id, order.CustomerId, order.CustomerName, order.PhoneNumber, order.Address, order.DeliveryTime, order.Items, order.Status, order.CreatedDate);
        }
    }
}
