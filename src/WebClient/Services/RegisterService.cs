using WebClient.Infrastructure;
using WebClient.Responses;

namespace WebClient.Services
{
    public class RegisterService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        private const string BaseUrl = "/identity-service/account/register";

        public async Task<RegisterResponse> RegisterAsync(RegisterDto registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}", registerDto);
            
            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                return new RegisterResponse(message, true);
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                return new RegisterResponse(message, false);
            }
        }
    }
}
