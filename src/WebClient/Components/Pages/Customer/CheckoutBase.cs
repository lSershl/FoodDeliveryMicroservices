using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using WebClient.Infrastructure;
using WebClient.Models;
using WebClient.Services;

namespace WebClient.Components.Pages.Customer
{
    public class CheckoutBase : ComponentBase
    {
        [CascadingParameter]
        protected Task<AuthenticationState>? AuthStateTask { get; init; }
        [Inject]
        public required BasketService BasketService { get; init; }
        [Inject]
        public required AddressService AddressService { get; init; }
        [Inject]
        public required PaymentCardService PaymentCardService { get; init; }
        [Inject]
        public required ILocalStorageService LocalStorage { get; init; }
        [Inject]
        public required IJSRuntime Js { get; init; }
        [Inject]
        public required NavigationManager NavigationManager { get; init; }

        protected Guid customerId;
        protected decimal totalPrice;

        protected bool step1 = true;
        protected bool step2 = false;
        protected bool step3 = false;
        
        protected CheckoutModel checkoutModel = new();

        protected List<BasketItem> basketItems = [];
        protected List<string> deliveryTimeList = ["13:00 - 15:00", "15:00 - 17:00", "17:00 - 19:00"];
        protected List<SavedAddressDto> savedAddresses = [];
        protected List<SavedPaymentCardDto> savedCards = [];

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (AuthStateTask is not null)
                {
                    var authState = await AuthStateTask;
                    var user = authState.User;
                    if (user.Identity!.IsAuthenticated)
                        customerId = Guid.Parse(user.Claims.First(x => x.Type.Contains("userdata"))!.Value);

                    checkoutModel.CustomerName = user.Claims.First(x => x.Type.Contains("name"))!.Value;
                    checkoutModel.PhoneNumber = user.Claims.First(x => x.Type.Contains("phone"))!.Value;
                }

                var token = await LocalStorage.GetItemAsync<string>("JWTToken");
                CustomerBasketDto basket = await BasketService.GetBasket(customerId, token!);
                totalPrice = CalculateTotalPrice(basket);

                basketItems = basket.Items;
                savedAddresses = await AddressService.GetSavedAddresses(customerId, token!);
                savedCards = await PaymentCardService.GetSavedCards(customerId, token!);
            }
            catch (Exception)
            {
                await Js.InvokeVoidAsync("alert", "Произошла ошибка, попробуйте позже");
                NavigationManager.Refresh(forceReload:true);
            }
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

        protected bool addNewAddressWindow = false;
        protected AddressModel newAddress = new();
        protected void ShowNewAddressWindow()
        {
            addNewAddressWindow = true;
        }

        protected void CloseNewAddressWindow()
        {
            addNewAddressWindow = false;
            newAddress.City = string.Empty;
            newAddress.Street = string.Empty;
            newAddress.House = string.Empty;
            newAddress.Apartment = string.Empty;
        }

        protected async void SaveNewAddress()
        {
            var token = await LocalStorage.GetItemAsync<string>("JWTToken");
            await AddressService.AddNewAddress(new NewAddressDto(
                customerId,
                newAddress.City,
                newAddress.Street,
                newAddress.House,
                newAddress.Apartment), token!);
            savedAddresses = await AddressService.GetSavedAddresses(customerId, token!);
            CloseNewAddressWindow();
        }

        protected void GoToStep1()
        {
            step1 = true;
            step2 = false;
            step3 = false;
        }

        protected async void GoToStep2()
        {
            if (checkoutModel.Address != string.Empty && checkoutModel.DeliveryTime != string.Empty)
            {
                step1 = false;
                step2 = true;
                step3 = false;
            }
            else
            {
                await Js.InvokeVoidAsync("alert", "Выберите адрес и время доставки!");
            }
        }

        protected Guid selectedCardId;
        protected async void SelectPaymentCard(Guid cardId)
        {
            var token = await LocalStorage.GetItemAsync<string>("JWTToken");
            PaymentCardModel card = await PaymentCardService.GetCardDetails(cardId, token!);
            if (card is not null)
            {
                checkoutModel.CardHolderName = card.CardHolderName;
                checkoutModel.CardNumber = card.CardNumber;
                checkoutModel.Expiration = card.Expiration;
                checkoutModel.Cvv = card.Cvv;
            }
        }

        protected bool addNewPaymentCardWindow = false;
        protected PaymentCardModel newPaymentCard = new();
        protected void ShowNewPaymentCardWindow()
        {
            addNewPaymentCardWindow = true;
        }

        protected void CloseNewPaymentCardWindow()
        {
            addNewPaymentCardWindow = false;
            newPaymentCard.CardNumber = string.Empty;
            newPaymentCard.CardHolderName = string.Empty;
            newPaymentCard.Expiration = string.Empty;
            newPaymentCard.Cvv = string.Empty;
        }

        protected async void SaveNewPaymentCard()
        {
            var token = await LocalStorage.GetItemAsync<string>("JWTToken");
            await PaymentCardService.AddNewPaymentCard(new NewPaymentCardDto(
                customerId,
                newPaymentCard.CardNumber,
                newPaymentCard.CardHolderName,
                newPaymentCard.Expiration,
                newPaymentCard.Cvv), token!);
            savedCards = await PaymentCardService.GetSavedCards(customerId, token!);
            CloseNewPaymentCardWindow();
        }

        protected async void GoToStep3()
        {
            if (checkoutModel.CardNumber != string.Empty &&
                checkoutModel.CardHolderName != string.Empty &&
                checkoutModel.Expiration != string.Empty &&
                checkoutModel.Cvv != string.Empty)
            {
                step1 = false;
                step2 = false;
                step3 = true;
            }
            else
            {
                await Js.InvokeVoidAsync("alert", "Выберите карту оплаты или добавьте новую!");
            }
        }

        protected async void ConfirmCheckout()
        {
            var token = await LocalStorage.GetItemAsync<string>("JWTToken");
            await BasketService.Checkout(customerId, new BasketCheckoutDto(
                totalPrice,
                checkoutModel.CustomerName,
                checkoutModel.PhoneNumber,
                checkoutModel.Address,
                checkoutModel.DeliveryTime,
                checkoutModel.Email,
                checkoutModel.CardHolderName,
                checkoutModel.CardNumber,
                checkoutModel.Expiration,
                checkoutModel.Cvv,
                basketItems,
                DateTimeOffset.Now), token!);

            NavigationManager.NavigateTo("/my-orders", true);
        }

        
    }
}
