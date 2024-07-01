using System.ComponentModel.DataAnnotations;

namespace WebClient.Models
{
    public class UserInfo
    {
        public required string Name { get; set; }
        public List<string>? SavedAddresses { get; set; } = new();
        public DateTime Birthday { get; set; }
        public string? Email { get; set; }
    }
}
