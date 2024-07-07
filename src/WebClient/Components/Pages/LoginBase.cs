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
        public required LoginService _loginService { get; set; }
        [Inject]
        public required IJSRuntime _js { get; set; }
        [Inject]
        public required AuthenticationStateProvider _authStateProvider { get; set; }
        [Inject]
        public required ILocalStorageService _localStorage { get; set; }
        [Inject]
        public required NavigationManager _navManager { get; set; }

        protected LoginModel Login = new();

        public async Task LoginButtonClicked()
        {
            LoginResponse response = await _loginService.LoginAsync(new LoginDto(Login.PhoneNumber, Login.Password));
            if (response is not null)
            {
                if (response!.Token is null)
                {
                    await _js.InvokeVoidAsync("alert", response.Message);
                    return;
                }

                await _js.InvokeVoidAsync("alert", response.Message);
            }
            

            await _localStorage.SetItemAsync("JWTToken", response.Token);
            var customAuthStateProvider = (CustomAuthenticationStateProvider)_authStateProvider;
            var token = await _localStorage.GetItemAsync<string>("JWTToken");
            customAuthStateProvider.UpdateAuthenticationState(token!);

            _navManager.NavigateTo("/", forceLoad: true);
        }
    }
}
