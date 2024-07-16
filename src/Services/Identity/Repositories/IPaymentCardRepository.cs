using Identity.Entities;

namespace Identity.Repositories
{
    public interface IPaymentCardRepository
    {
        Task<List<PaymentCard>> GetPaymentCardsByUserAsync(Guid customerId);
        Task<PaymentCard> GetPaymentCardByIdAsync(Guid cardId);
        void AddPaymentCard(PaymentCard paymentCard);
        void RemovePaymentCard(Guid customerId, string partialCardNumber);
    }
}
