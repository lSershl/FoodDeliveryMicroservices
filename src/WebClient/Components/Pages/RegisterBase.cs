using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using WebClient.Infrastructure;
using WebClient.Models;
using WebClient.Responses;
using WebClient.Services;

namespace WebClient.Components.Pages
{
    public class RegisterBase : ComponentBase
    {
        [Inject]
        public required RegisterService _registerService { get; init; }
        [Inject]
        public required IJSRuntime _js { get; init; }
        [Inject]
        public required NavigationManager _navManager { get; init; }

        protected RegisterModel RegModel = new();

        public async Task RegisterButtonClicked()
        {
            var response = await _registerService.RegisterAsync(
                new RegisterDto(
                    RegModel.PhoneNumber,
                    RegModel.Password,
                    RegModel.Name,
                    RegModel.Birthday.ToUniversalTime().Date,
                    RegModel.Email));

            if (response is not null)
            {
                if (response.IsSuccessful)
                {
                    await _js.InvokeVoidAsync("alert", response.Message);
                    _navManager.NavigateTo("/login", forceLoad: true);
                }
                else
                {
                    await _js.InvokeVoidAsync("alert", response.Message);
                    return;
                }   
            }
        }
    }
}
