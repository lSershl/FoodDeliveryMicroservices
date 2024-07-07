using Basket.Entities;
using Basket.Infrastructure;

namespace Basket.Data
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(Guid customerId);
        Task<CustomerBasket> StoreBasketAsync(CustomerBasketDto basketDto);
        Task<bool> DeleteBasketAsync(Guid customerId);
    }
}
