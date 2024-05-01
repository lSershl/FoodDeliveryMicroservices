using Microsoft.AspNetCore.Components;
using WebClient.Infrastructure;
using WebClient.Models;
using WebClient.Services;

namespace WebClient.Components.Pages
{
    public class CustomerBasketBase : ComponentBase
    {
        [Inject]
        public required BasketService BasketService { get; set; }
        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        protected Guid customerId = Guid.Parse("11a4eb82-356d-421d-9d97-2cbb73881111"); // hardcoded customerId, later will be passed as part of auth token
        protected List<BasketItem> basketItems = new();
        
        protected override async Task OnInitializedAsync()
        {
            var customerBasket = await BasketService.GetBasket(customerId);
            if (customerBasket.Items.Count > 0)
            {
                foreach (var dtoItem in customerBasket.Items)
                {
                    basketItems.Add(new BasketItem
                    {
                        Id = dtoItem.Id,
                        ProductId = dtoItem.ProductId,
                        Name = dtoItem.Name,
                        Price = dtoItem.Price,
                        Quantity = dtoItem.Quantity,
                        ImageUrl = dtoItem.ImageUrl
                    });
                }
                CalculateSummary();
            }
        }

        protected void AddQuantityAndSaveBasketChanges(Guid itemId)
        {
            basketItems.First(x => x.Id == itemId).Quantity++;
            SaveBasketChanges();
            CalculateSummary();
        }

        protected void RemoveQuantityAndSaveBasketChanges(Guid itemId)
        {
            basketItems.First(x => x.Id == itemId).Quantity--;
            SaveBasketChanges();
            CalculateSummary();
        }

        protected void SaveBasketChanges()
        {
            CustomerBasketDto changedBasket = new CustomerBasketDto(customerId, basketItems);
            BasketService.StoreBasket(changedBasket);
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

        protected void ClearBasket()
        {
            BasketService.ClearBasket(customerId);
            NavigationManager.NavigateTo("/basket", true);
        }
    }
}
