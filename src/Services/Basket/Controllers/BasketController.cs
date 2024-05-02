using Basket.Data;
using Basket.Entities;
using Basket.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Controllers
{
    [ApiController]
    [Route("basket")]
    public class BasketController(IBasketRepository basketRepository) : ControllerBase
    {
        private readonly IBasketRepository _basketRepository = basketRepository;

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
    }
}
