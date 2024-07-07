using Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Ordering.Entities;
using Ordering.Infrastructure;

namespace Ordering.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController(IRepository<Order> repository, IPublishEndpoint publishEndpoint) : ControllerBase
    {
        private readonly IRepository<Order> _repository = repository;
        public readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        [HttpGet("for_customer/{customerId}")]
        public async Task<ActionResult> GetCustomerOrdersAsync(Guid customerId)
        {
            var items = (await _repository.GetAllAsync()).Where(x => x.CustomerId == customerId).Select(a => a.AsDto());
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderByIdAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            if (item is null)
            {
                return NotFound("Заказ не найден!");
            }
            return Ok(item!.AsDto());
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewOrderAsync(CreateOrderDto createOrderDto)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = createOrderDto.CustomerName,
                PhoneNumber = createOrderDto.PhoneNumber,
                Address = createOrderDto.Address,
                DeliveryTime = createOrderDto.DeliveryTime,
                Items = createOrderDto.Items,
                Status = "Создан",
                CreatedDate = DateTimeOffset.UtcNow
            };
            await _repository.CreateAsync(order);
            await _publishEndpoint.Publish(new OrderCreated(order.Id, order.CustomerName, order.PhoneNumber, order.Address, order.DeliveryTime, order.Items, order.CreatedDate));
            return CreatedAtAction(nameof(GetOrderByIdAsync), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrderAsync(Guid id, UpdateOrderDto updateOrderDto)
        {
            var existingOrder = await _repository.GetAsync(id);
            if (existingOrder is null)
            {
                return NotFound("Заказ не найден!");
            }
            existingOrder.PhoneNumber = updateOrderDto.PhoneNumber;
            existingOrder.Address = updateOrderDto.Address;
            existingOrder.DeliveryTime = updateOrderDto.DeliveryTime;
            existingOrder.Items = updateOrderDto.Items;
            await _repository.UpdateAsync(existingOrder);
            await _publishEndpoint.Publish(new OrderUpdated(existingOrder.Id, existingOrder.CustomerName, existingOrder.PhoneNumber, existingOrder.Address, existingOrder.DeliveryTime, existingOrder.Items, existingOrder.CreatedDate));
            return Ok("Заказ обновлён");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrderAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            if (item is null)
            {
                return NotFound("Заказ не найден!");
            }
            await _repository.RemoveAsync(item.Id);
            await _publishEndpoint.Publish(new OrderDeleted(id));
            return Ok("Заказ удалён");
        }
    }
}
