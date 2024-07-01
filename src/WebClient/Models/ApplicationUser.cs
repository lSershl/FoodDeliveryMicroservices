
namespace WebClient.Models
{
    public class ApplicationUser
    {
        public required UserInfo UserInfo { get; set; }
        public List<PaymentCardInfo>? PaymentCards { get; init; }
    }
}
