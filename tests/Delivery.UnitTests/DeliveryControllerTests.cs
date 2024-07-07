using AutoFixture;
using Delivery.Controllers;
using Delivery.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Delivery.Infrastructure;

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
        public void GetDeliveriesAsync_ReturnsOk()
        {
            // Arrange
            Guid courierId = Guid.NewGuid();

            // Act
            var response = _deliveryController.GetDeliveriesAsync(courierId);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void CreateOrUpdateDeliveryAsync_ReturnsOk()
        {
            // Arrange
            var fixture = new Fixture();
            var grantOrderDto = fixture.Create<GrantOrderDto>();

            // Act
            var response = _deliveryController.CreateOrUpdateDeliveryAsync(grantOrderDto);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}