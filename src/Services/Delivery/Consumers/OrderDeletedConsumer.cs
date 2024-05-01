using Delivery.Entities;
using Infrastructure;
using MassTransit;

namespace Delivery.Consumers
{
    public class OrderDeletedConsumer(IRepository<Order> repository) : IConsumer<OrderDeleted>
    {
        private readonly IRepository<Order> _repository = repository;

        public async Task Consume(ConsumeContext<OrderDeleted> context)
        {
            var message = context.Message;
            var item = await _repository.GetAsync(message.Id);

            if (item is null)
            {
                return;
            }

            await _repository.RemoveAsync(message.Id);
        }
    }
}
