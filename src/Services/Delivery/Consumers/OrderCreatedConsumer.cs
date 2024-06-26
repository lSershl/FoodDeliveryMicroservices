﻿using Delivery.Entities;
using Infrastructure;
using MassTransit;

namespace Delivery.Consumers
{
    public class OrderCreatedConsumer(IRepository<Order> repository) : IConsumer<OrderCreated>
    {
        private readonly IRepository<Order> _repository = repository;
        public async Task Consume(ConsumeContext<OrderCreated> context)
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
        }
    }
}
