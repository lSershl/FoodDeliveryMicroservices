using Microsoft.AspNetCore.Components;
using WebClient.Infrastructure;
using WebClient.Services;
using WebClient.Models;
using WebClient.Components.Pages.Customer;

namespace WebClient.Components.Pages
{
    public class CatalogBase : ComponentBase
    {
        [Inject]
        public required CatalogService CatalogService { get; set; }
        [Inject]
        public required BasketService BasketService { get; set; }

        protected IEnumerable<CatalogItemDto>? catalogItems = new List<CatalogItemDto>();
        protected Guid customerId = Guid.Parse("11a4eb82-356d-421d-9d97-2cbb73881111"); // hardcoded customerId, later will be passed as part of auth token
        protected CustomerBasket currentBasket = new();

        protected override async Task OnInitializedAsync()
        {
            catalogItems = await CatalogService.GetCatalogItems();

            var storedBasket = await BasketService.GetBasket(customerId);
            currentBasket.CustomerId = customerId;

            if (storedBasket.Items.Count == 0)
                return;
            else
            {
                foreach (var item in storedBasket.Items)
                {
                    currentBasket.Items!.Add(item);
                }
            }
        }

        protected void AddToBasket(CatalogItemDto catalogItem)
        {
            BasketItem newItem = new BasketItem
            {
                ProductId = catalogItem.Id,
                Name = catalogItem.Name,
                Price = catalogItem.Price,
                PictureUrl = catalogItem.PictureUrl,
                Quantity = 1
            };

            if (currentBasket.Items!.Count == 0)
            {
                currentBasket.Items!.Add(newItem);
                BasketService.StoreBasket(new CustomerBasketDto(currentBasket.CustomerId, currentBasket.Items!));
            }
            else
            {
                var item = currentBasket.Items!.FirstOrDefault(x => x.ProductId == newItem.ProductId, null);
                if (item is null)
                    currentBasket.Items!.Add(newItem);
                else
                    currentBasket.Items.First(x => x.ProductId == newItem.ProductId).Quantity++;
                BasketService.StoreBasket(new CustomerBasketDto(currentBasket.CustomerId, currentBasket.Items));
            }
        }
    }
}
