using Delivery.Entities;
using Delivery.Processors;
using Infrastructure;
using MassTransit;

namespace Delivery.Consumers
{
    public class OrderPreparedConsumer(IRepository<Order> repository, DeliveryProcessor deliveryProcessor) : IConsumer<OrderPrepared>
    {
        private readonly IRepository<Order> _repository = repository;
        private readonly DeliveryProcessor _deliveryProcessor = deliveryProcessor;

        public async Task Consume(ConsumeContext<OrderPrepared> context)
        {
            var message = context.Message;
            var item = await _repository.GetAsync(message.Id);

            if (item is not null)
            {
                return;
            }

            item = new Order
            {
                Id = message.Id,
                CustomerName = message.CustomerName,
                PhoneNumber = message.PhoneNumber,
                Address = message.Address,
                DeliveryTime = message.DeliveryTime,
                Items = message.Items,
                Status = message.Status,
                CreatedDate = message.CreatedDate
            };

            await _repository.CreateAsync(item);
            await _deliveryProcessor.AcceptOrderToDelivery(item.Id);
        }
    }
}
