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
    public class AddressControllerTests
    {
        private readonly AddressController _addressController;
        private readonly IAddressRepository _repository;

        public AddressControllerTests()
        {
            // Dependencies
            _repository = Substitute.For<IAddressRepository>();

            // System Under Test
            _addressController = new AddressController(_repository);
        }

        [Fact]
        public void GetUserAddresses_ReturnsOk()
        {
            // Arrange
            Guid customerId = Guid.NewGuid();
            List<Address> addresses = new();
            var fixture = new Fixture();
            for (int i = 0; i < 5; i++)
            {
                addresses.Add(fixture.Build<Address>().Without(x => x.User).Create());
            }
            _repository.GetAddressesByUserAsync(customerId).Returns(addresses);

            // Act
            var response = _addressController.GetUserAddresses(customerId);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void SaveNewAddressForUser_ReturnsOk()
        {
            // Arrange
            var fixture = new Fixture();
            var addressDto = fixture.Create<NewAddressDto>();

            // Act
            var response = _addressController.SaveNewAddressForUser(addressDto);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void DeleteSavedAddress_ReturnsOk()
        {
            // Arrange
            var fixture = new Fixture();
            Guid customerId = Guid.NewGuid();
            string address = fixture.Create<string>();

            // Act
            var response = _addressController.DeleteSavedAddress(customerId, address);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}
