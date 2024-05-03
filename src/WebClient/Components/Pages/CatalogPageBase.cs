using Microsoft.AspNetCore.Components;
using WebClient.Infrastructure;
using WebClient.Services;
using WebClient.Models;

namespace WebClient.Components.Pages
{
    public class CatalogPageBase : ComponentBase
    {
        [Inject]
        public required CatalogService CatalogService { get; set; }
        [Inject]
        public required BasketService BasketService { get; set; }

        protected IEnumerable<CatalogItemDto>? catalogItems = new List<CatalogItemDto>();
        protected Guid customerId = Guid.Parse("11a4eb82-356d-421d-9d97-2cbb73881111"); // hardcoded customerId, later will be passed as part of auth token

        protected override async Task OnInitializedAsync()
        {
            catalogItems = await CatalogService.GetCatalogItems();
        }

        protected async void AddToBasket(CatalogItemDto item)
        {
            CustomerBasketDto basket = await BasketService.GetBasket(customerId);

            foreach (var basketItem in basket.Items)
            {
                if (basketItem.ProductId == item.Id)
                    basketItem.Quantity++;
                else
                {
                    basket.Items.Add(new BasketItem
                    {
                        ProductId = item.Id,
                        Name = item.Name,
                        Price = item.Price,
                        ImageUrl = item.ImageUrl,
                        Quantity = 1
                    });
                }
            }
            BasketService.StoreBasket(basket);
        }
    }
}
