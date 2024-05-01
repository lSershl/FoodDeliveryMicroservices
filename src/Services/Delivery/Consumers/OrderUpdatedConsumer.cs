using Delivery.Entities;
using Infrastructure;
using MassTransit;

namespace Delivery.Consumers
{
    public class OrderUpdatedConsumer(IRepository<Order> repository) : IConsumer<OrderUpdated>
    {
        private readonly IRepository<Order> _repository = repository;

        public async Task Consume(ConsumeContext<OrderUpdated> context)
        {
            var message = context.Message;
            var item = await _repository.GetAsync(message.Id);

            if (item is null)
            {
                item = new Order
                {
                    Id = message.Id,
                    Address = message.Address,
                    Quantity = message.Quantity
                };
                await _repository.CreateAsync(item);
            }
            else
            {
                item.Address = message.Address;
                item.Quantity = message.Quantity;

                await _repository.UpdateAsync(item);
            }
        }
    }
}
