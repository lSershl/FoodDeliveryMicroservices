using MassTransit.Transports;
using MassTransit;
using Ordering.Entities;
using Infrastructure;

namespace Ordering.Processors
{
    public class OrderingProcessor(IRepository<Order> repository, IPublishEndpoint publishEndpoint)
    {
        private readonly IRepository<Order> _repository = repository;
        public readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        public async Task AcceptOrder(Guid orderId)
        {
            var newOrder = await _repository.GetAsync(orderId);
            newOrder.Status = "Принят";
            await _repository.UpdateAsync(newOrder);

            Task.Delay(3000).Wait(); // Simulate latency while registering a new order from customer

            await PrepareOrder(orderId);

            await SendOrderToDelivery(orderId);
        }

        private async Task PrepareOrder(Guid orderId)
        {
            var order = await _repository.GetAsync(orderId);
            order.Status = "Готовим";
            await _repository.UpdateAsync(order);

            Task.Delay(5000).Wait(); // Simulate food cooking process
        }

        private async Task SendOrderToDelivery(Guid orderId)
        {
            var order = await _repository.GetAsync(orderId);
            order.Status = "В пути";
            await _repository.UpdateAsync(order);

            await _publishEndpoint.Publish(new OrderPrepared(
                orderId, 
                order.CustomerName,
                order.PhoneNumber, 
                order.Address, 
                order.DeliveryTime, 
                order.Items, 
                order.Status, 
                order.CreatedDate));
        }
    }
}
