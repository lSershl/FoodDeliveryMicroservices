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

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Обязательное поле")]
        protected string address = string.Empty;

        public async Task RegisterButtonClicked()
        {
            ServiceResponse response = await _registerService.RegisterAsync(
                new RegisterDto(
                    RegModel.PhoneNumber,
                    RegModel.Password,
                    RegModel.Name,
                    RegModel.Birthday.ToUniversalTime(),
                    RegModel.Email));

            if (response is not null)
            {
                await _js.InvokeVoidAsync("alert", response.Message);
            }

            _navManager.NavigateTo("/login", forceLoad: true);
        }
    }
}
