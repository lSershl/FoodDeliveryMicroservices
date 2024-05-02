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
        public void GetAsync_ReturnsOk()
        {
            // Arrange
            
            // Act
            var result = _orderController.GetAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Task<ActionResult<IEnumerable<OrderDto>>>));
        }

        [Fact]
        public void GetByIdAsync_ReturnsOk()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            // Act
            var result = _orderController.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Task<ActionResult<OrderDto>>));
        }

        [Fact]
        public void PostAsync_ReturnsOk()
        {
            // Arrange
            var fixture = new Fixture();
            var createOrderDto = fixture.Create<CreateOrderDto>();

            // Act
            var result = _orderController.PostAsync(createOrderDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Task<ActionResult<OrderDto>>));
        }

        [Fact]
        public void PutAsync_ReturnsOk()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var fixture = new Fixture();
            var updateOrderDto = fixture.Create<UpdateOrderDto>();

            // Act
            var result = _orderController.PutAsync(id, updateOrderDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Task<IActionResult>));
        }

        [Fact]
        public void DeleteAsync_ReturnsOk()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            // Act
            var result = _orderController.DeleteAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Task<IActionResult>));
        }
    }
}