using Basket.Data;
using Basket.Entities;
using Basket.Infrastructure;
using Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Controllers
{
    [ApiController]
    [Route("basket")]
    public class BasketController(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint) : ControllerBase
    {
        private readonly IBasketRepository _basketRepository = basketRepository;
        public readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        [HttpGet("{customerId}")]
        public async Task<ActionResult<CustomerBasketDto>> GetBasket(Guid customerId)
        {
            var basket = await _basketRepository.GetBasketAsync(customerId);
            if (basket is null)
            {
                return new CustomerBasket(customerId).AsDto();
            }
            return basket.AsDto();
        }

        [HttpPost("{customerId}")]
        public async Task<ActionResult<CustomerBasketDto>> StoreBasket(CustomerBasketDto customerBasketDto)
        {
            var storedBasket = await _basketRepository.StoreBasketAsync(new CustomerBasket
            {
                CustomerId = customerBasketDto.CustomerId,
                Items = customerBasketDto.Items
            });
            if (storedBasket is null)
            {
                return BadRequest("Не удалось сохранить корзину");
            }
            return storedBasket.AsDto();
        }

        [HttpDelete("{customerId}")]
        public async Task<ActionResult> DeleteBasket(Guid customerId)
        {
            var result = await _basketRepository.DeleteBasketAsync(customerId);
            if (result)
                return Ok("Корзина была очищена");
            return BadRequest("Не удалось очистить корзину");
        }

        [HttpPost("checkout/{customerId}")]
        public async Task<IActionResult> Checkout(Guid customerId, BasketCheckoutDto basketCheckoutDto)
        {
            await _publishEndpoint.Publish(new BasketCheckoutCompleted(
                customerId,
                basketCheckoutDto.CustomerName, 
                basketCheckoutDto.TotalPrice, 
                basketCheckoutDto.Address, 
                basketCheckoutDto.PhoneNumber, 
                basketCheckoutDto.DeliveryTime, 
                basketCheckoutDto.Items.AsDto(), 
                basketCheckoutDto.CreatedDate));
            await _basketRepository.DeleteBasketAsync(customerId);
            return Ok();
        }
    }
}
