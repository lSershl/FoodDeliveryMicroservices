using Infrastructure;
using MassTransit;
using Ordering.Entities;
using Ordering.Processors;

namespace Ordering.Consumers
{
    public class BasketCheckoutCompletedConsumer(IRepository<Order> repository, OrderingProcessor orderingProcessor) : IConsumer<BasketCheckoutCompleted>
    {
        private readonly IRepository<Order> _repository = repository;
        private readonly OrderingProcessor _orderingProcessor = orderingProcessor;

        public async Task Consume(ConsumeContext<BasketCheckoutCompleted> context)
        {
            var message = context.Message;

            var item = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = message.CustomerId,
                CustomerName = message.CustomerName,
                PhoneNumber = message.PhoneNumber,
                Address = message.Address,
                DeliveryTime = message.DeliveryTime,
                Items = message.Items,
                Status = "Оплачен",
                CreatedDate = message.CreatedDate
            };

            await _repository.CreateAsync(item);

            await _orderingProcessor.AcceptOrder(item.Id);
        }
    }
}
