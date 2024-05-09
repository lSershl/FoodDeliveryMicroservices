using Infrastructure;
using MassTransit;
using Ordering.Entities;

namespace Ordering.Consumers
{
    public class OrderDeliveryCompletedConsumer(IRepository<Order> repository) : IConsumer<OrderDeliveryCompleted>
    {
        private readonly IRepository<Order> _repository = repository;

        public async Task Consume(ConsumeContext<OrderDeliveryCompleted> context)
        {
            var message = context.Message;
            var order = await _repository.GetAsync(message.OrderId);
            order.Status = "Доставлен";
            await _repository.UpdateAsync(order);
        }
    }
}
