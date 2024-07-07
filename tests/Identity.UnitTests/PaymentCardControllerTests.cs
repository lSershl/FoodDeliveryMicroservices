using AutoFixture;
using FluentAssertions;
using Identity.Controllers;
using Identity.Entities;
using Identity.Infrastructure;
using Identity.Repositories;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Identity.UnitTests
{
    public class PaymentCardControllerTests
    {
        private readonly PaymentCardController _paymentCardController;
        private readonly IPaymentCardRepository _repository;

        public PaymentCardControllerTests()
        {
            // Dependencies
            _repository = Substitute.For<IPaymentCardRepository>();

            // System Under Test
            _paymentCardController = new PaymentCardController(_repository);
        }

        [Fact]
        public void GetUserSavedCards_ReturnsOk()
        {
            // Arrange
            Guid customerId = Guid.NewGuid();
            List<PaymentCard> cards = new();
            _repository.GetPaymentCardsByUserAsync(customerId).Returns(cards);

            // Act
            var response = _paymentCardController.GetUserSavedCards(customerId);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void SaveNewPaymentCardForUser_ReturnsOk()
        {
            // Arrange
            var fixture = new Fixture();
            var newPaymentCardDto = fixture.Create<NewPaymentCardDto>();

            // Act
            var response = _paymentCardController.SaveNewPaymentCardForUser(newPaymentCardDto);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void DeleteSavedCard_ReturnsOk()
        {
            // Arrange
            Guid customerId = Guid.NewGuid();
            var fixture = new Fixture();
            string partialCardNumber = fixture.Create<string>();

            // Act
            var response = _paymentCardController.DeleteSavedCard(customerId, partialCardNumber);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}
