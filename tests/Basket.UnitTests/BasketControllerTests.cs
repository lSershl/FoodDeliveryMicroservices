using AutoFixture;
using Basket.Controllers;
using Basket.Data;
using Basket.Entities;
using Basket.Infrastructure;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Basket.UnitTests
{
    public class BasketControllerTests
    {
        private BasketController _basketController;
        private IBasketRepository _repository;
        private IPublishEndpoint _publishEndpoint;

        public BasketControllerTests()
        {
            // Dependencies
            _repository = Substitute.For<IBasketRepository>();
            _publishEndpoint = Substitute.For<IPublishEndpoint>();

            // System Under Test
            _basketController = new BasketController(_repository, _publishEndpoint);
        }

        [Fact]
        public void GetBasket_ReturnsOk()
        {
            // Arrange
            Guid customerId = Guid.NewGuid();
            var fixture = new Fixture();
            _repository.GetBasketAsync(customerId).Returns(fixture.Create<CustomerBasket>());

            // Act
            var response = _basketController.GetBasket(customerId);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void StoreBasket_ReturnsOk()
        {
            // Arrange
            var fixture = new Fixture();
            var customerBasketDto = fixture.Create<CustomerBasketDto>();
            _repository.StoreBasketAsync(customerBasketDto).Returns(fixture.Create<CustomerBasket>());

            // Act
            var response = _basketController.StoreBasket(customerBasketDto);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void DeleteBasket_ReturnsOk()
        {
            // Arrange
            Guid customerId = Guid.NewGuid();
            _repository.DeleteBasketAsync(customerId).Returns(true);

            // Act
            var response = _basketController.DeleteBasket(customerId);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void Checkout_ReturnsOk()
        {
            // Arrange
            Guid customerId = Guid.NewGuid();
            var fixture = new Fixture();
            var basketCheckoutDto = fixture.Create<BasketCheckoutDto>();

            // Act
            var response = _basketController.Checkout(customerId, basketCheckoutDto);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}