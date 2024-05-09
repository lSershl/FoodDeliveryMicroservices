using WebClient.Infrastructure;

namespace WebClient.Services
{
    public class OrderService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        private const string BaseUrl = "/ordering-service/order";

        public async Task<IEnumerable<OrderDto>> GetCustomerOrders(Guid customerId)
        {
            var response = await _httpClient.GetAsync(BaseUrl);
            var result = await response.Content.ReadAsStringAsync();
            return [.. Extensions.JSONSerializer.DeserializeJsonStringEnum<OrderDto>(result)];
        }
    }
}
