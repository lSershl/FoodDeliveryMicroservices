using WebClient.Infrastructure;
using WebClient.Responses;

namespace WebClient.Services
{
    public class RegisterService(IHttpClientFactory factory)
    {
        private readonly HttpClient client = factory.CreateClient("YARPGateway");
        private const string BaseUrl = "/identity-service/account/register";

        public async Task<RegisterResponse> RegisterAsync(RegisterDto registerDto)
        {
            var response = await client.PostAsJsonAsync($"{BaseUrl}", registerDto);
            
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
