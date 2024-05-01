using Basket.Entities;
using StackExchange.Redis;
using System.Text.Json;

namespace Basket.Data
{
    public class BasketRepository(IConnectionMultiplexer redis) : IBasketRepository
    {
        private readonly IDatabase _redisDb = redis.GetDatabase();
        
        public async Task<CustomerBasket?> GetBasketAsync(Guid customerId)
        {
            var basket = await _redisDb.StringGetAsync(customerId.ToString());
            return basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket> StoreBasketAsync(CustomerBasket basket)
        {
            var updateOrCreateBasket = await _redisDb.StringSetAsync(basket.CustomerId.ToString(), JsonSerializer.Serialize(basket), TimeSpan.FromDays(1));
            if (updateOrCreateBasket)
                return await GetBasketAsync(basket.CustomerId);
            return null!;
        }
        public async Task<bool> DeleteBasketAsync(Guid customerId)
        {
            return await _redisDb.KeyDeleteAsync(customerId.ToString());
        }
    }
}
