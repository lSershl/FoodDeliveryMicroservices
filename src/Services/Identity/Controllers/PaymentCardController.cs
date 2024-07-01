using Identity.Entities;
using Identity.Infrastructure;
using Identity.Repositories;
using Identity.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [ApiController]
    [Route("cards")]
    public class PaymentCardController(IPaymentCardRepository paymentCardRepository) : ControllerBase
    {
        private readonly IPaymentCardRepository _paymentCardRepository = paymentCardRepository;

        [HttpGet("for_customer/{customerId}")]
        [Authorize]
        public async Task<ActionResult<List<SavedPaymentCardDto>>> GetUserSavedCards(Guid customerId)
        {
            List<SavedPaymentCardDto> userCards = new();
            var result = await _paymentCardRepository.GetPaymentCardsByUserAsync(customerId);
            foreach (var card in result)
            {
                userCards.Add(new SavedPaymentCardDto(string.Concat("****-****-****-", card.CardNumber.AsSpan(card.CardNumber.Length - 4, 4))));
            }
            return Ok(userCards);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ServiceResponce>> SaveNewPaymentCardForUser(NewPaymentCardDto newPaymentCardDto)
        {
            _paymentCardRepository.AddPaymentCard(new PaymentCard
            {
                Id = Guid.NewGuid(),
                CustomerId = newPaymentCardDto.CustomerId,
                CardHolderName = newPaymentCardDto.CardHolderName,
                CardNumber = newPaymentCardDto.CardNumber,
                Expiration  = newPaymentCardDto.Expiration,
                Cvv = newPaymentCardDto.Cvv
            });
            return new ServiceResponce("Карта сохранена");
        }

        [HttpDelete("{customerId}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponce>> DeleteSavedCard(Guid customerId, string PartialCardNumber)
        {
            _paymentCardRepository.RemovePaymentCard(customerId, PartialCardNumber);
            return new ServiceResponce("Карта удалена");
        }
    }
}
