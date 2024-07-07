using AutoFixture;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Ordering.Controllers;
using Ordering.Entities;
using Ordering.Infrastructure;

namespace Ordering.UnitTests
{
    public class OrderControllerTests
    {
        private OrderController _orderController;
        private IRepository<Order> _repository;
        private IPublishEndpoint _publishEndpoint;

        public OrderControllerTests()
        {
            // Dependencies
            _repository = Substitute.For<IRepository<Order>>();
            _publishEndpoint = Substitute.For<IPublishEndpoint>();

            // System Under Test
            _orderController = new OrderController(
                _repository,
                _publishEndpoint);
        }

        [Fact]
        public void GetCustomerOrdersAsync_ReturnsOk()
        {
            // Arrange
            Guid customerId = Guid.NewGuid();

            // Act
            var response = _orderController.GetCustomerOrdersAsync(customerId);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void GetOrderByIdAsync_ReturnsOk()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var fixture = new Fixture();
            _repository.GetAsync(id).Returns(fixture.Create<Order>());

            // Act
            var response = _orderController.GetOrderByIdAsync(id);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void CreateNewOrderAsync_ReturnsOk()
        {
            // Arrange
            var fixture = new Fixture();
            var createOrderDto = fixture.Create<CreateOrderDto>();

            // Act
            var response = _orderController.CreateNewOrderAsync(createOrderDto);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(CreatedAtActionResult));
        }

        [Fact]
        public void UpdateOrderAsync_ReturnsOk()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var fixture = new Fixture();
            var updateOrderDto = fixture.Create<UpdateOrderDto>();
            _repository.GetAsync(id).Returns(fixture.Create<Order>());

            // Act
            var response = _orderController.UpdateOrderAsync(id, updateOrderDto);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void DeleteOrderAsync_ReturnsOk()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var fixture = new Fixture();
            _repository.GetAsync(id).Returns(fixture.Create<Order>());

            // Act
            var response = _orderController.DeleteOrderAsync(id);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}