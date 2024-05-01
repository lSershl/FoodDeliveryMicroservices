using Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Ordering.Entities;
using Ordering.Infrastructure;

namespace Ordering.Controllers
{
    [ApiController]
    [Route("order")]
    public class OrderController(IRepository<Order> repository, IPublishEndpoint publishEndpoint) : ControllerBase
    {
        private readonly IRepository<Order> _repository = repository;
        public readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAsync()
        {
            var items = (await _repository.GetAllAsync()).Select(a => a.AsDto());
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetByIdAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            if (item is null)
            {
                return NotFound();
            }
            return item!.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> PostAsync(CreateOrderDto createOrderDto)
        {
            var order = new Order
            {
                Address = createOrderDto.Address,
                Quantity = createOrderDto.Quantity,
                CreatedDate = DateTimeOffset.UtcNow,
            };
            await _repository.CreateAsync(order);
            await _publishEndpoint.Publish(new OrderCreated(order.Id, order.Address, order.Quantity, order.CreatedDate));
            return CreatedAtAction(nameof(GetByIdAsync), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateOrderDto updateItemDto)
        {
            var existingOrder = await _repository.GetAsync(id);
            if (existingOrder is null)
            {
                return NotFound();
            }
            existingOrder.Address = updateItemDto.Address;
            existingOrder.Quantity = updateItemDto.Quantity;
            await _repository.UpdateAsync(existingOrder);
            await _publishEndpoint.Publish(new OrderUpdated(existingOrder.Id, existingOrder.Address, existingOrder.Quantity, existingOrder.CreatedDate));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            if (item is null)
            {
                return NotFound();
            }
            await _repository.RemoveAsync(item.Id);
            await _publishEndpoint.Publish(new OrderDeleted(id));
            return NoContent();
        }
    }
}
