using System.Net.Http.Headers;
using WebClient.Infrastructure;
using WebClient.Models;
using WebClient.Responses;

namespace WebClient.Services
{
    public class PaymentCardService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        private const string BaseUrl = "/identity-service/cards";

        public async Task<List<SavedPaymentCardDto>> GetSavedCards(Guid customerId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{BaseUrl}/for_customer/{customerId}");
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
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{BaseUrl}/id/{cardId}");
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
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsync($"{BaseUrl}",
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
