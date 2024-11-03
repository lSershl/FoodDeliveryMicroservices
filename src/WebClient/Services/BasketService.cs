using System.Net.Http.Headers;
using WebClient.Infrastructure;
using WebClient.Responses;

namespace WebClient.Services
{
    public class BasketService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        private const string BaseUrl = "/basket-service/basket";

        public async Task<CustomerBasketDto> GetBasket(Guid customerId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{BaseUrl}/{customerId}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return null!;
                }

                var result = await response.Content.ReadAsStringAsync();
                return Extensions.JSONSerializer.DeserializeJsonString<CustomerBasketDto>(result);
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    return null!;
                }
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }

        public async Task<ServiceResponse> StoreBasket(CustomerBasketDto customerBasketDto, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsync($"{BaseUrl}", 
                Extensions.JSONSerializer.GenerateStringContent(
                    Extensions.JSONSerializer.SerializeObj(customerBasketDto)));

            if (response.IsSuccessStatusCode)
            {
                return new ServiceResponse("Корзина сохранена");
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }

        public async Task<ServiceResponse> ClearBasket(Guid customerId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{customerId}");

            if (response.IsSuccessStatusCode)
            {
                return new ServiceResponse("Корзина очищена");
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }

        public async Task<ServiceResponse> Checkout(Guid customerId, BasketCheckoutDto basketCheckoutDto, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsync($"{BaseUrl}/checkout/{customerId}",
                Extensions.JSONSerializer.GenerateStringContent(
                    Extensions.JSONSerializer.SerializeObj(basketCheckoutDto)));

            if (response.IsSuccessStatusCode)
            {
                return new ServiceResponse("Заказ успешно оформлен");
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }
    }
}
