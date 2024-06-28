using WebClient.Infrastructure;
using WebClient.Responses;

namespace WebClient.Services
{
    public class RegisterService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        private const string BaseUrl = "/identity-service/register";

        public async Task<ServiceResponse> RegisterAsync(RegisterDto registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}", registerDto);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse>();
            return result!;
        }
    }
}
