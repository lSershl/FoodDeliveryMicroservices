using System.ComponentModel.DataAnnotations;

namespace Identity.Entities
{
    public class PaymentCard
    {
        [Key]
        public required Guid Id { get; set; }
        public required Guid CustomerId { get; set; }
        public required string CardNumber { get; set; }
        public required string CardHolderName { get; set; }
        public required string Expiration { get; set; }
        public required string Cvv { get; set; }
        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "Неправильный формат срока действия карты!")]
        

        public User User { get; set; } = null!;
    }
}
