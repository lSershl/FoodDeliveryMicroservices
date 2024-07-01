using System.ComponentModel.DataAnnotations;

namespace Identity.Entities
{
    public class User
    {
        [Key]
        public required Guid CustomerId { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string? Email { get; set; } = string.Empty;

        public List<Address>? SavedAddresses { get; set; } = new();
        public List<PaymentCard>? SavedPaymentCards { get; set; } = new();
    }
}
