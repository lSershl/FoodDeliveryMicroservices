using System.Net.Http.Headers;
using WebClient.Infrastructure;

namespace WebClient.Services
{
    public class OrderService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        private const string BaseUrl = "/ordering-service/orders/for_customer";

        public async Task<IEnumerable<OrderDto>> GetCustomerOrders(Guid customerId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{BaseUrl}/{customerId}");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<OrderDto>();
                }

                var result = await response.Content.ReadAsStringAsync();
                return [.. Extensions.JSONSerializer.DeserializeJsonStringEnum<OrderDto>(result)];
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
            //var result = await response.Content.ReadAsStringAsync();
            //return [.. Extensions.JSONSerializer.DeserializeJsonStringEnum<OrderDto>(result)];
        }
    }
}
