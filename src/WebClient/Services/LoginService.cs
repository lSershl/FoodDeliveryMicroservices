using WebClient.Infrastructure;
using WebClient.Responses;

namespace WebClient.Services
{
    public class LoginService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        private const string BaseUrl = "/identity-service/account/login";

        public async Task<LoginResponse> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, loginDto);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result!;
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
            
        }
    }
}
