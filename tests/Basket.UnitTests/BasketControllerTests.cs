using AutoFixture;
using Basket.Controllers;
using Basket.Data;
using Basket.Entities;
using Basket.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Basket.UnitTests
{
    public class BasketControllerTests
    {
        private BasketController _basketController;
        private IBasketRepository _repository;

        public BasketControllerTests()
        {
            // Dependencies
            _repository = Substitute.For<IBasketRepository>();

            // System Under Test
            _basketController = new BasketController(_repository);
        }

        [Fact]
        public void GetBasket_ReturnsOk()
        {
            // Arrange
            Guid customerId = Guid.NewGuid();
            var fixture = new Fixture();
            _repository.GetBasketAsync(customerId).Returns(fixture.Create<CustomerBasket>());

            // Act
            var result = _basketController.GetBasket(customerId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Task<ActionResult<CustomerBasketDto>>));
        }

        [Fact]
        public void StoreBasket_ReturnsOk()
        {
            // Arrange
            var fixture = new Fixture();
            var customerBasketDto = fixture.Create<CustomerBasketDto>();

            // Act
            var result = _basketController.StoreBasket(customerBasketDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Task<ActionResult<CustomerBasketDto>>));
        }

        [Fact]
        public void DeleteBasket_ReturnsOk()
        {
            // Arrange
            Guid customerId = Guid.NewGuid();

            // Act
            var result = _basketController.DeleteBasket(customerId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Task<ActionResult>));
        }
    }
}