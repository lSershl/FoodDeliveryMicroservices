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

        public static List<BasketItemDto> AsDto(this List<BasketItem> basketItems)
        {
            var basketItemsDto = new List<BasketItemDto>();
            foreach (var basketItem in basketItems)
            {
                basketItemsDto.Add(AsDto(basketItem));
            }
            return basketItemsDto;
        }
    }
}
