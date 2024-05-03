using Microsoft.AspNetCore.Mvc;
using static Delivery.Infrastructure.Dtos;
using Delivery.Entities;
using Delivery.Infrastructure;

namespace Delivery.Controllers
{
    [ApiController]
    [Route("delivery")]
    public class DeliveryController(IRepository<CourierDelivery> repository, IRepository<Order> orderRepository) : ControllerBase
    {
        private readonly IRepository<CourierDelivery> _repository = repository;
        private readonly IRepository<Order> _orderRepository = orderRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryDto>>> GetAsync(Guid courierId)
        {
            if (courierId == Guid.Empty)
            {
                return BadRequest();
            }

            var deliveryEntities = await _repository.GetAllAsync(a => a.CourierId == courierId);
            var itemIds = deliveryEntities.Select(a => a.OrderId);
            var orderEntities = await _orderRepository.GetAllAsync(a => itemIds.Contains(a.Id));

            var deliveryDtos = deliveryEntities.Select(a =>
            {
                var orderItem = orderEntities.Single(b => b.Id == a.OrderId);
                return a.AsDto(orderItem.Address, orderItem.DeliveryTime);
            });

            return Ok(deliveryDtos);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantOrderDto grantOrderDto)
        {
            var delivery = await _repository.GetAsync(a => a.CourierId == grantOrderDto.CourierId && a.OrderId == grantOrderDto.OrderId);
            if (delivery is null)
            {
                delivery = new CourierDelivery
                {
                    CourierId = grantOrderDto.CourierId,
                    OrderId = grantOrderDto.OrderId,
                    Status = grantOrderDto.Status,
                    AcquiredDate = DateTimeOffset.UtcNow
                };

                await _repository.CreateAsync(delivery);
            }
            else
            {
                delivery.Status = grantOrderDto.Status;
                await _repository.UpdateAsync(delivery);
            }
            return Ok();
        }
    }
}
