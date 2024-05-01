using Basket.Entities;

namespace Basket.Data
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(Guid customerId);
        Task<CustomerBasket> StoreBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(Guid customerId);
    }
}
