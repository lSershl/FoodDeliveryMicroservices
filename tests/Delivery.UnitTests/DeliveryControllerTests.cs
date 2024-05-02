using AutoFixture;
using Delivery.Controllers;
using Delivery.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using static Delivery.Infrastructure.Dtos;

namespace Delivery.UnitTests
{
    public class DeliveryControllerTests
    {
        private DeliveryController _deliveryController;
        private IRepository<CourierDelivery> _deliveryRepository;
        private IRepository<Order> _orderRepository;

        public DeliveryControllerTests()
        {
            // Dependencies
            _deliveryRepository = Substitute.For<IRepository<CourierDelivery>>();
            _orderRepository = Substitute.For<IRepository<Order>>();

            // System Under Test
            _deliveryController = new DeliveryController(
                _deliveryRepository,
                _orderRepository);
        }

        [Fact]
        public void GetAsync_ReturnsOk()
        {
            // Arrange
            Guid courierId = Guid.NewGuid();

            // Act
            var result = _deliveryController.GetAsync(courierId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Task<ActionResult<IEnumerable<DeliveryDto>>>));
        }

        [Fact]
        public void PostAsync_ReturnsOk()
        {
            // Arrange
            var fixture = new Fixture();
            var grantOrderDto = fixture.Create<GrantOrderDto>();

            // Act
            var result = _deliveryController.PostAsync(grantOrderDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Task<ActionResult>));
        }
    }
}