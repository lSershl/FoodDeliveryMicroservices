using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using WebClient.Infrastructure;
using WebClient.Models;
using WebClient.Services;

namespace WebClient.Components.Pages.Customer
{
    public class BasketCheckoutBase : ComponentBase
    {
        [CascadingParameter]
        protected Task<AuthenticationState>? authStateTask { get; init; }
        [Inject]
        public required BasketService BasketService { get; set; }
        [Inject]
        public required ILocalStorageService localStorage { get; set; }
        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        protected Guid customerId = Guid.Empty;
        public BasketCheckoutModel? basketCheckoutModel = new BasketCheckoutModel();
        protected List<string> deliveryTimeList = new List<string> { "13:00 - 15:00", "15:00 - 17:00", "17:00 - 19:00"};
        protected decimal totalPrice { get; set; }

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

                    basketCheckoutModel!.CustomerName = user.Claims.First(x => x.Type.Contains("name"))!.Value;
                    basketCheckoutModel!.PhoneNumber = user.Claims.First(x => x.Type.Contains("phone"))!.Value;
                    basketCheckoutModel!.Address = user.Claims.First(x => x.Type.Contains("address"))!.Value;
                }

                var token = await localStorage.GetItemAsync<string>("JWTToken");
                var basket = await BasketService.GetBasket(customerId, token!);
                totalPrice = CalculateTotalPrice(basket);
            }
            catch (Exception)
            {
                //NavigationManager.Refresh(true);
            }
            
        }

        protected async Task ConfirmCheckout()
        {
            var token = await localStorage.GetItemAsync<string>("JWTToken");
            var basket = await BasketService.GetBasket(customerId, token!);
            totalPrice = CalculateTotalPrice(basket);
            await BasketService.Checkout(customerId, new BasketCheckoutDto(
                totalPrice,
                basketCheckoutModel!.CustomerName,
                basketCheckoutModel.PhoneNumber,
                basketCheckoutModel.Address,
                basketCheckoutModel.DeliveryTime,
                basketCheckoutModel.Email,
                basketCheckoutModel.CardHolderName,
                basketCheckoutModel.CardNumber,
                basketCheckoutModel.Expiration,
                basketCheckoutModel.Cvv,
                basket.Items,
                DateTimeOffset.Now));
            NavigationManager.NavigateTo("/basket", true);
        }

        protected decimal CalculateTotalPrice(CustomerBasketDto basket)
        {
            decimal totalPrice = 0;
            foreach (var item in basket.Items)
            {
                totalPrice += item.Price * item.Quantity;
            }
            return totalPrice;
        }
    }
}
