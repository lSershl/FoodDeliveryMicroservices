using System.ComponentModel.DataAnnotations;

namespace Identity.Entities
{
    public class ApplicationUser
    {
        [Key]
        public Guid CustomerId { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public DateTime Birthday { get; set; }
        public string? Email { get; set; } = string.Empty;
    }
}
