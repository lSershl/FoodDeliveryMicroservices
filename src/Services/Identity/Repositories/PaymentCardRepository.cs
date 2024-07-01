using Identity.Data;
using Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repositories
{
    public class PaymentCardRepository(ApplicationDbContext dbContext) : IPaymentCardRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<List<PaymentCard>> GetPaymentCardsByUserAsync(Guid customerId)
        {
            var cards = await _dbContext.SavedUserPaymentCards.Where(x => x.CustomerId == customerId).ToListAsync();
            return cards;
        }

        public void AddPaymentCard(PaymentCard paymentCard)
        {
            _dbContext.SavedUserPaymentCards.Add(paymentCard);
            _dbContext.SaveChanges();
        }

        public void RemovePaymentCard(Guid customerId, string partialCardNumber)
        {
            var paymentCards = _dbContext.SavedUserPaymentCards.Where(x => x.CustomerId == customerId).ToList();
            var cardToDelete = paymentCards.Find(x => x.CardNumber.Contains(partialCardNumber));
            if (cardToDelete != null)
            {
                _dbContext.SavedUserPaymentCards.Remove(cardToDelete);
                _dbContext.SaveChanges();
            }
        }
    }
}
