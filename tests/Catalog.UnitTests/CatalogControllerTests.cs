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
        public void GetCatalogAsync_ReturnsOk()
        {
            // Arrange

            // Act
            var response = _catalogController.GetCatalogAsync();

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void GetCatalogItemByIdAsync_ReturnsOk()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var fixture = new Fixture();
            _repository.GetAsync(id).Returns(fixture.Create<CatalogItem>());

            // Act
            var response = _catalogController.GetCatalogItemByIdAsync(id);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void PostCatalogItemAsync_ReturnsOk()
        {
            // Arrange
            var fixture = new Fixture();
            var createCatalogItemDto = fixture.Create<CreateCatalogItemDto>();

            // Act
            var response = _catalogController.PostCatalogItemAsync(createCatalogItemDto);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(CreatedAtActionResult));
        }

        [Fact]
        public void UpdateCatalogItemAsync_ReturnsOk()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var fixture = new Fixture();
            var updateOrderDto = fixture.Create<UpdateCatalogItemDto>();
            _repository.GetAsync(id).Returns(fixture.Create<CatalogItem>());

            // Act
            var response = _catalogController.UpdateCatalogItemAsync(id, updateOrderDto);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void DeleteCatalogItemAsync_ReturnsOk()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var fixture = new Fixture();
            _repository.GetAsync(id).Returns(fixture.Create<CatalogItem>());

            // Act
            var response = _catalogController.DeleteCatalogItemAsync(id);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}