using Delivery.Entities;
using Infrastructure;
using MassTransit;

namespace Delivery.Processors
{
    public class DeliveryProcessor(IRepository<Order> orderRepository, IRepository<CourierDelivery> courierDeliveryRepository, IPublishEndpoint publishEndpoint)
    {
        private readonly IRepository<Order> _orderRepository = orderRepository;
        private readonly IRepository<CourierDelivery> _courierDeliveryRepository = courierDeliveryRepository;
        public readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        public async Task AcceptOrderToDelivery(Guid orderId)
        {
            var newOrder = await _orderRepository.GetAsync(orderId);
            newOrder.Status = "Принят для доставки";
            await _orderRepository.UpdateAsync(newOrder);

            var newDeliveryId = await AssignOrderToCourier(newOrder.Id);
            await CompleteDelivery(newDeliveryId);
        }

        private async Task<Guid> AssignOrderToCourier(Guid orderId)
        {
            var order = await _orderRepository.GetAsync(orderId);
            var delivery = new CourierDelivery
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                CourierId = Guid.NewGuid(),
                Status = order.Status,
                AcquiredDate = DateTimeOffset.UtcNow
            };
            await _courierDeliveryRepository.CreateAsync(delivery);

            return delivery.Id;
        }

        private async Task CompleteDelivery(Guid deliveryId)
        {
            Task.Delay(5000).Wait(); // Simulate delivery to the customer
            var delivery = await _courierDeliveryRepository.GetAsync(deliveryId);
            var order = await _orderRepository.GetAsync(delivery.OrderId);
            delivery.Status = "Завершена";
            order.Status = "Доставлен";

            await _orderRepository.UpdateAsync(order);
            await _courierDeliveryRepository.UpdateAsync(delivery);

            await _publishEndpoint.Publish(new OrderDeliveryCompleted(order.Id));
        }
    }
}
