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
                    CustomerName = message.CustomerName,
                    PhoneNumber = message.PhoneNumber,
                    Address = message.Address,
                    DeliveryTime = message.DeliveryTime,
                    Items = message.Items,
                    Status = message.Status,
                    CreatedDate = message.CreatedDate
                };
                await _repository.CreateAsync(item);
            }
            else
            {
                item.PhoneNumber = message.PhoneNumber;
                item.Address = message.Address;
                item.DeliveryTime = message.DeliveryTime;
                item.Items = message.Items;
                await _repository.UpdateAsync(item);
            }
        }
    }
}
