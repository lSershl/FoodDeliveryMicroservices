using Basket.Entities;

namespace Basket.Infrastructure
{
    public static class Extensions
    {
        public static BasketItemDto AsDto(this BasketItem basketItem)
        {
            return new BasketItemDto(basketItem.ProductId, basketItem.Name, basketItem.Price, basketItem.Quantity, basketItem.ImageUrl);
        }

        public static CustomerBasketDto AsDto(this CustomerBasket customerBasket)
        {
            return new CustomerBasketDto(customerBasket.CustomerId, customerBasket.Items);
        }
    }
}
