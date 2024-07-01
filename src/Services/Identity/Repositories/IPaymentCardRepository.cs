using Identity.Entities;

namespace Identity.Repositories
{
    public interface IPaymentCardRepository
    {
        Task<List<PaymentCard>> GetPaymentCardsByUserAsync(Guid customerId);
        void AddPaymentCard(PaymentCard paymentCard);
        void RemovePaymentCard(Guid customerId, string partialCardNumber);
    }
}
