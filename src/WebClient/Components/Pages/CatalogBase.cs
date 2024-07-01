using Microsoft.AspNetCore.Components;
using WebClient.Infrastructure;
using WebClient.Services;
using WebClient.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

namespace WebClient.Components.Pages
{
    public class CatalogBase : ComponentBase
    {
        [CascadingParameter]
        protected Task<AuthenticationState>? authStateTask { get; init; }
        [Inject]
        public required CatalogService CatalogService { get; set; }
        [Inject]
        public required ILocalStorageService localStorage { get; set; }
        [Inject]
        public required BasketService BasketService { get; set; }

        protected IEnumerable<CatalogItemDto>? catalogItems = new List<CatalogItemDto>();
        protected Guid customerId = Guid.Empty;
        protected CustomerBasket currentBasket = new();

        protected override async Task OnInitializedAsync()
        {
            catalogItems = await CatalogService.GetCatalogItems();
            if (authStateTask is not null)
            {
                var authState = await authStateTask;
                var user = authState.User;
                if (user is not null && user.Identity!.IsAuthenticated)
                {
                    customerId = Guid.Parse(user.Claims.First(x => x.Type.Contains("userdata"))!.Value);
                    var token = await localStorage.GetItemAsync<string>("JWTToken");
                    var storedBasket = await BasketService.GetBasket(customerId, token!);
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
            }

            
        }

        protected async void AddToBasket(CatalogItemDto catalogItem)
        {
            BasketItem newItem = new()
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
                await BasketService.StoreBasket(new CustomerBasketDto(currentBasket.CustomerId, currentBasket.Items!));
            }
            else
            {
                var item = currentBasket.Items!.FirstOrDefault(x => x.ProductId == newItem.ProductId, null);
                if (item is null)
                    currentBasket.Items!.Add(newItem);
                else
                    currentBasket.Items.First(x => x.ProductId == newItem.ProductId).Quantity++;
                await BasketService.StoreBasket(new CustomerBasketDto(currentBasket.CustomerId, currentBasket.Items));
            }
        }
    }
}
