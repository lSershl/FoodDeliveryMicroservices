using WebClient.Infrastructure;
using WebClient.Responses;

namespace WebClient.Services
{
    public class LoginService(IHttpClientFactory factory)
    {
        private readonly HttpClient client = factory.CreateClient("YARPGateway");
        private const string BaseUrl = "/identity-service/account/login";

        public async Task<LoginResponse> LoginAsync(LoginDto loginDto)
        {
            var response = await client.PostAsJsonAsync(BaseUrl, loginDto);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result!;
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                return new LoginResponse(message, null!);
            }
        }
    }
}
