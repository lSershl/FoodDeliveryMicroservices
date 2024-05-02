using AutoFixture;
using Catalog.Controllers;
using Catalog.Data;
using Catalog.Entities;
using Catalog.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Catalog.UnitTests
{
    public class CatalogControllerTests
    {
        private CatalogController _catalogController;
        private IRepository<CatalogItem> _repository;
        private CatalogInitialData _initialData;

        public CatalogControllerTests()
        {
            // Dependencies
            _repository = Substitute.For<IRepository<CatalogItem>>();
            _initialData = Substitute.For<CatalogInitialData>();

            // System Under Test
            _catalogController = new CatalogController(
                _repository,
                _initialData);
        }

        [Fact]
        public void GetAsync_ReturnsOk()
        {
            // Arrange

            // Act
            var result = _catalogController.GetAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Task<ActionResult<IEnumerable<CatalogItemDto>>>));
        }

        [Fact]
        public void GetByIdAsync_ReturnsOk()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var fixture = new Fixture();
            _repository.GetAsync(id).Returns(fixture.Create<CatalogItem>());

            // Act
            var result = _catalogController.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Task<ActionResult<CatalogItemDto>>));
        }

        [Fact]
        public void PostAsync_ReturnsOk()
        {
            // Arrange
            var fixture = new Fixture();
            var createCatalogItemDto = fixture.Create<CreateCatalogItemDto>();

            // Act
            var result = _catalogController.PostAsync(createCatalogItemDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Task<ActionResult<CatalogItemDto>>));
        }

        [Fact]
        public void PutAsync_ReturnsOk()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var fixture = new Fixture();
            var updateOrderDto = fixture.Create<UpdateCatalogItemDto>();

            // Act
            var result = _catalogController.PutAsync(id, updateOrderDto);

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
            var result = _catalogController.DeleteAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Task<IActionResult>));
        }
    }
}