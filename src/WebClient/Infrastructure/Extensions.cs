using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using WebClient.Models;

namespace WebClient.Infrastructure
{
    public static class Extensions
    {
        public static BasketItemDto AsDto(this BasketItem basketItem)
        {
            return new BasketItemDto(basketItem.ProductId, basketItem.Name, basketItem.Price, basketItem.Quantity, basketItem.PictureUrl);
        }

        public static class JSONSerializer
        {
            public static string SerializeObj(object obj) => JsonSerializer.Serialize(obj, JsonOptions());
            public static T DeserializeJsonString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString, JsonOptions())!;
            public static StringContent GenerateStringContent(string serializedObj) => new(serializedObj, Encoding.UTF8, "application/json");
            public static IList<T> DeserializeJsonStringList<T>(string jsonString) => JsonSerializer.Deserialize<IList<T>>(jsonString, JsonOptions())!;
            public static IEnumerable<T> DeserializeJsonStringEnum<T>(string jsonString) => JsonSerializer.Deserialize<IEnumerable<T>>(jsonString, JsonOptions())!;
            private static JsonSerializerOptions JsonOptions()
            {
                return new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
                };
            }
        }
    }
}
