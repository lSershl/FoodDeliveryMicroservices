using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using WebClient.Infrastructure;
using WebClient.Models;
using WebClient.Responses;
using WebClient.Services;
using WebClient.States;

namespace WebClient.Components.Pages
{
    public class LoginBase : ComponentBase
    {
        [Inject]
        public required LoginService loginService { get; set; }
        [Inject]
        public required IJSRuntime js { get; set; }
        [Inject]
        public required AuthenticationStateProvider authStateProvider { get; set; }
        [Inject]
        public required ILocalStorageService localStorage { get; set; }
        [Inject]
        public required NavigationManager navManager { get; set; }

        protected LoginModel Login = new();

        public async Task LoginButtonClicked()
        {
            LoginResponse response = await loginService.LoginAsync(new LoginDto(Login.PhoneNumber, Login.Password));
            if (response is not null)
            {
                await js.InvokeVoidAsync("alert", response.Message);
            }

            await localStorage.SetItemAsync("JWTToken", response.Token);
            var customAuthStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
            var token = await localStorage.GetItemAsync<string>("JWTToken");
            customAuthStateProvider.UpdateAuthenticationState(token!);

            navManager.NavigateTo("/", forceLoad: true);
        }
    }
}
