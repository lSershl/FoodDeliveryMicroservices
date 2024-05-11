using Microsoft.AspNetCore.Components;
using WebClient.Infrastructure;
using WebClient.Models;
using WebClient.Services;

namespace WebClient.Components.Pages.Customer
{
    public class BasketCheckoutBase : ComponentBase
    {
        [Inject]
        public required BasketService BasketService { get; set; }
        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        protected Guid customerId = Guid.Parse("11a4eb82-356d-421d-9d97-2cbb73881111"); // hardcoded customerId, later will be passed as part of auth token
        public BasketCheckoutModel? basketCheckoutModel = new BasketCheckoutModel();
        protected List<string> deliveryTimeList = new List<string> { "13:00 - 15:00", "15:00 - 17:00", "17:00 - 19:00"};
        protected decimal totalPrice { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var basket = await BasketService.GetBasket(customerId);
                totalPrice = CalculateTotalPrice(basket);
            }
            catch (Exception)
            {
                NavigationManager.Refresh(true);
            }
            
        }

        protected async Task ConfirmCheckout()
        {
            var basket = await BasketService.GetBasket(customerId);
            totalPrice = CalculateTotalPrice(basket);
            BasketService.Checkout(customerId, new BasketCheckoutDto(
                totalPrice,
                basketCheckoutModel!.CustomerName,
                basketCheckoutModel.PhoneNumber,
                basketCheckoutModel.Address,
                basketCheckoutModel.DeliveryTime,
                basketCheckoutModel.Email,
                basketCheckoutModel.CardName,
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
