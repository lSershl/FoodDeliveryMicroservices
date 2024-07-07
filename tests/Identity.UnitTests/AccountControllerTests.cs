using AutoFixture;
using FluentAssertions;
using Identity.Controllers;
using Identity.Entities;
using Identity.Infrastructure;
using Identity.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Identity.UnitTests
{
    public class AccountControllerTests
    {
        [Fact]
        public void Login_WithCorrectCredentials_ReturnsOk()
        {
            // Arrange
            var repository = Substitute.For<IAccountRepository>();
            const string jwtKey = "1111111111111111111111111111111111111111";
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"Jwt:Key", jwtKey}
                })
                .Build();
            var accountController = new AccountController(repository, configuration);

            var fixture = new Fixture();
            var loginDto = new LoginDto("+78001234455", "12345");
            var user = fixture.Build<User>()
                .Without(x => x.SavedAddresses)
                .Without(x => x.SavedPaymentCards)
                .Create();
            user.PhoneNumber = "+78001234455";
            user.Password = "12345";

            repository.GetUserByPhoneAsync(loginDto.PhoneNumber).Returns(user);

            // Act
            var response = accountController.Login(loginDto);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void Login_WithInvalidPassword_ReturnsBadRequest()
        {
            // Arrange
            var repository = Substitute.For<IAccountRepository>();
            var configuration = Substitute.For<IConfiguration>();
            var accountController = new AccountController(repository, configuration);

            var fixture = new Fixture();
            var user = fixture.Build<User>()
                .Without(x => x.SavedAddresses)
                .Without(x => x.SavedPaymentCards)
                .Create();
            var loginDto = new LoginDto("+78001234455", "12345");
            user.PhoneNumber = "+78001234455";
            user.Password = "password";
            repository.GetUserByPhoneAsync(loginDto.PhoneNumber).Returns(user);

            // Act
            var response = accountController.Login(loginDto);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public void Login_WithNonExistUser_ReturnsNotFound()
        {
            // Arrange
            var repository = Substitute.For<IAccountRepository>();
            var configuration = Substitute.For<IConfiguration>();
            var accountController = new AccountController(repository, configuration);

            var fixture = new Fixture();
            var loginDto = new LoginDto("+78001234455", "12345");

            repository.GetUserByPhoneAsync(loginDto.PhoneNumber).ReturnsNull();

            // Act
            var response = accountController.Login(loginDto);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(NotFoundObjectResult));
        }

        [Fact]
        public void Register_WithCorrectData_ReturnsOk()
        {
            // Arrange
            var repository = Substitute.For<IAccountRepository>();
            var configuration = Substitute.For<IConfiguration>();
            var accountController = new AccountController(repository, configuration);

            var fixture = new Fixture();
            var registerDto = fixture.Create<RegisterDto>();

            // Act
            var response = accountController.Register(registerDto);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData("+78001234455", "", "")]
        [InlineData("", "password", "")]
        [InlineData("", "", "name")]
        public void Register_WithMissingData_ReturnsBadRequest(string phoneNumber, string password, string name)
        {
            // Arrange
            var repository = Substitute.For<IAccountRepository>();
            var configuration = Substitute.For<IConfiguration>();
            var accountController = new AccountController(repository, configuration);
            var registerDto = new RegisterDto(phoneNumber, password, name, DateTime.UtcNow, string.Empty);

            // Act
            var response = accountController.Register(registerDto);

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType(typeof(BadRequestObjectResult));
        }
    }
}