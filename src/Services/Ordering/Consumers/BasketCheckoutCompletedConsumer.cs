using Infrastructure;
using MassTransit;
using Ordering.Entities;

namespace Ordering.Consumers
{
    public class BasketCheckoutCompletedConsumer(IRepository<Order> repository) : IConsumer<BasketCheckoutCompleted>
    {
        private readonly IRepository<Order> _repository = repository;

        public async Task Consume(ConsumeContext<BasketCheckoutCompleted> context)
        {
            var message = context.Message;

            var item = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = message.CustomerName,
                PhoneNumber = message.PhoneNumber,
                Address = message.Address,
                DeliveryTime = message.DeliveryTime,
                Items = message.Items,
                Status = "Created",
                CreatedDate = message.CreatedDate
            };

            await _repository.CreateAsync(item);
        }
    }
}
