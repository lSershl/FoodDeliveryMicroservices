using System.Net.Http.Headers;
using WebClient.Infrastructure;
using WebClient.Models;
using WebClient.Responses;
using Extensions = WebClient.Infrastructure.Extensions;

namespace WebClient.Services
{
    public class PaymentCardService(IHttpClientFactory factory)
    {
        private readonly HttpClient client = factory.CreateClient("YARPGateway");
        private const string BaseUrl = "/identity-service/cards";

        public async Task<List<SavedPaymentCardDto>> GetSavedCards(Guid customerId, string token)
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
                return [.. Extensions.JSONSerializer.DeserializeJsonStringList<SavedPaymentCardDto>(result)];
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }

        public async Task<PaymentCardModel> GetCardDetails(Guid cardId, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{BaseUrl}/id/{cardId}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default!;
                }
                var result = await response.Content.ReadAsStringAsync();
                return Extensions.JSONSerializer.DeserializeJsonString<PaymentCardModel>(result);
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }

        public async Task<ServiceResponse> AddNewPaymentCard(NewPaymentCardDto newPaymentCardDto, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"{BaseUrl}",
                Extensions.JSONSerializer.GenerateStringContent(
                    Extensions.JSONSerializer.SerializeObj(newPaymentCardDto)));
            if (response.IsSuccessStatusCode)
            {
                return new ServiceResponse("Карта сохранена");
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }
    }
}
