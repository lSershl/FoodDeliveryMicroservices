using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using WebClient.Infrastructure;
using WebClient.Models;
using WebClient.Services;

namespace WebClient.Components.Pages.Customer
{
    public class BasketBase : ComponentBase
    {
        [CascadingParameter]
        protected Task<AuthenticationState>? authStateTask { get; init; }
        [Inject]
        public required BasketService BasketService { get; set; }
        [Inject]
        public required ILocalStorageService localStorage { get; set; }
        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        // hardcoded customerId, later will be passed as part of auth token
        //protected Guid customerId = Guid.Parse("a05a70d2-6b85-4cea-91f5-3501cf827a7f");
        protected Guid customerId = Guid.Empty;
        protected List<BasketItem> basketItems = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (authStateTask is not null)
                {
                    var authState = await authStateTask;
                    var user = authState.User;
                    if (user.Identity!.IsAuthenticated)
                        customerId = Guid.Parse(user.Claims.First(x => x.Type.Contains("userdata"))!.Value);
                }

                var token = await localStorage.GetItemAsync<string>("JWTToken");
                var customerBasket = await BasketService.GetBasket(customerId, token!);
                if (customerBasket.Items.Count > 0)
                {
                    foreach (var dtoItem in customerBasket.Items)
                    {
                        basketItems.Add(new BasketItem
                        {
                            ProductId = dtoItem.ProductId,
                            Name = dtoItem.Name,
                            Price = dtoItem.Price,
                            Quantity = dtoItem.Quantity,
                            PictureUrl = dtoItem.PictureUrl
                        });
                    }
                    CalculateSummary();
                }
            }
            catch (Exception)
            {

                NavigationManager.Refresh();
            }
            
        }

        protected void AddQuantityAndSaveBasketChanges(Guid productId)
        {
            basketItems.First(x => x.ProductId == productId).Quantity++;
            SaveBasketChanges();
            CalculateSummary();
        }

        protected void RemoveQuantityAndSaveBasketChanges(Guid productId)
        {
            var item = basketItems.First(x => x.ProductId == productId);
            item.Quantity--;

            if (item.Quantity == 0)
            {
                basketItems.Remove(item);
            }
            else
            {
                basketItems.First(x => x.ProductId == item.ProductId).Quantity--;
            }
            SaveBasketChanges();
            CalculateSummary();
        }

        protected async void SaveBasketChanges()
        {
            CustomerBasketDto changedBasket = new CustomerBasketDto(customerId, basketItems);
            await BasketService.StoreBasket(changedBasket);
        }

        protected decimal summary = 0;
        protected void CalculateSummary()
        {
            summary = 0;
            foreach (var item in basketItems)
            {
                summary += item.Price * item.Quantity;
            }
        }

        protected async void ClearBasket()
        {
            await BasketService.ClearBasket(customerId);
            NavigationManager.NavigateTo("/basket", true);
        }

        protected void GoToCheckout()
        {
            NavigationManager.NavigateTo("/checkout");
        }
    }
}
