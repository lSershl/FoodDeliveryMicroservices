using WebClient.Infrastructure;

namespace WebClient.Services
{
    public class BasketService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        private const string BaseUrl = "/basket-service/basket";

        public async Task<CustomerBasketDto> GetBasket(Guid customerId)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{customerId}");
            var result = await response.Content.ReadAsStringAsync();
            return Extensions.JSONSerializer.DeserializeJsonString<CustomerBasketDto>(result);
        }

        public async void StoreBasket(CustomerBasketDto customerBasketDto)
        {
            var response = await _httpClient.PostAsync($"{BaseUrl}/{customerBasketDto.CustomerId}", 
                Extensions.JSONSerializer.GenerateStringContent(
                    Extensions.JSONSerializer.SerializeObj(customerBasketDto)));
        }

        public async void ClearBasket(Guid customerId)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{customerId}");
        }

        public async void Checkout(Guid customerId, BasketCheckoutDto basketCheckoutDto)
        {
            var response = await _httpClient.PostAsync($"{BaseUrl}/checkout/{customerId}",
                Extensions.JSONSerializer.GenerateStringContent(
                    Extensions.JSONSerializer.SerializeObj(basketCheckoutDto)));
        }
    }
}
