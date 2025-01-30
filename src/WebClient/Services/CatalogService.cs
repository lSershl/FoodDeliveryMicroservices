using WebClient.Infrastructure;
using Extensions = WebClient.Infrastructure.Extensions;

namespace WebClient.Services
{
    public class CatalogService(IHttpClientFactory factory)
    {
        private readonly HttpClient client = factory.CreateClient("YARPGateway");
        private const string BaseUrl = "/catalog-service/catalog";

        public async Task<IEnumerable<CatalogItemDto>> GetCatalogItems()
        {
            var response = await client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<CatalogItemDto>();
                }

                var result = await response.Content.ReadAsStringAsync();
                return [.. Extensions.JSONSerializer.DeserializeJsonStringEnum<CatalogItemDto>(result)];
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            } 
        }
    }
}
