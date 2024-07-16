using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace WebClient.Components.Pages.Customer
{
    public class AccountBase : ComponentBase
    {
        [CascadingParameter]
        protected Task<AuthenticationState>? AuthStateTask { get; init; }
        [Inject]
        public required NavigationManager NavigationManager { get; init; }

        protected string Name = string.Empty;
        protected string Birthday = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (AuthStateTask is not null)
                {
                    var authState = await AuthStateTask;
                    var user = authState.User;
                    if (user.Identity!.IsAuthenticated)
                    {
                        Name = user.Claims.First(x => x.Type.Contains("name"))!.Value;
                        Birthday = DateTime.Parse(user.Claims.First(x => x.Type.Contains("birth"))!.Value).ToShortDateString();
                    }
                    else
                        NavigationManager.NavigateTo("/login", true);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void GoToOrders()
        {
            NavigationManager.NavigateTo("/my-orders", true);
        }
    }
}
