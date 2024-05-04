using Microsoft.AspNetCore.Components;
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
    }
}
