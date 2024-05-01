using WebClient.Infrastructure;

namespace WebClient.Services
{
    public class CatalogService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        private const string BaseUrl = "/catalog-service/catalog";

        public async Task<IEnumerable<CatalogItemDto>> GetCatalogItems()
        {
            var response = await _httpClient.GetAsync(BaseUrl);
            var result = await response.Content.ReadAsStringAsync();
            return [.. Extensions.JSONSerializer.DeserializeJsonStringEnum<CatalogItemDto>(result)];
        }
    }
}
