using Delivery.Entities;
using static Delivery.Infrastructure.Dtos;

namespace Delivery.Infrastructure
{
    public static class Extensions
    {
        public static DeliveryDto AsDto(this CourierDelivery delivery, string Address, int Quantity)
        {
            return new DeliveryDto(delivery.OrderId, Address, Quantity, delivery.Status!, delivery.Id, delivery.CourierId, delivery.AcquiredDate);
        }
    }
}
