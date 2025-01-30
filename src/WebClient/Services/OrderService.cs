using System.Net.Http.Headers;
using WebClient.Infrastructure;
using Extensions = WebClient.Infrastructure.Extensions;

namespace WebClient.Services
{
    public class OrderService(IHttpClientFactory factory)
    {
        private readonly HttpClient client = factory.CreateClient("YARPGateway");

        private const string BaseUrl = "/ordering-service/orders/for_customer";

        public async Task<IEnumerable<OrderDto>> GetCustomerOrders(Guid customerId, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"{BaseUrl}/{customerId}");

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
        }
    }
}
