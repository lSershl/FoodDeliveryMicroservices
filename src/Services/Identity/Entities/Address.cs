using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Entities
{
    public class Address
    {
        [Key]
        public required Guid Id { get; set; }
        public required Guid CustomerId { get; set; }
        public required string City { get; set; }
        public required string Street { get; set; }
        public required string House { get; set; }
        public required string Apartment { get; set; }
        public required string FullAddress { get; set; }

        public User User { get; set; } = null!;
    }
}
