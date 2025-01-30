using System.Net.Http.Headers;
using WebClient.Infrastructure;
using WebClient.Responses;
using Extensions = WebClient.Infrastructure.Extensions;

namespace WebClient.Services
{
    public class AddressService(IHttpClientFactory factory)
    {
        private readonly HttpClient client = factory.CreateClient("YARPGateway");
        private const string BaseUrl = "/identity-service/addresses";

        public async Task<List<SavedAddressDto>> GetSavedAddresses(Guid customerId, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{BaseUrl}/for_customer/{customerId}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default!;
                }
                var result = await response.Content.ReadAsStringAsync();
                return [.. Extensions.JSONSerializer.DeserializeJsonStringList<SavedAddressDto>(result)];
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }

        public async Task<ServiceResponse> AddNewAddress(NewAddressDto newAddressDto, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"{BaseUrl}", 
                Extensions.JSONSerializer.GenerateStringContent(
                    Extensions.JSONSerializer.SerializeObj(newAddressDto)));
            if (response.IsSuccessStatusCode)
            {
                return new ServiceResponse("Адрес сохранён");
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }
    }
}
