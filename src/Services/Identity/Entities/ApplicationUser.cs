using System.ComponentModel.DataAnnotations;

namespace Identity.Entities
{
    public class ApplicationUser
    {
        public required User UserInfo { get; init; }
        public required List<Address> AddressList { get; init; }  
        public List<PaymentCard>? PaymentCards { get; init; }
    }
}
