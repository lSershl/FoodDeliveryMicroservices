using Identity.Entities;
using Identity.Infrastructure;
using Identity.Repositories;
using Identity.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController(IAccountRepository accountRepository, IConfiguration configuration) : ControllerBase
    {
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly IConfiguration _configuration = configuration;

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var user = await _accountRepository.GetUserByPhoneAsync(loginDto.PhoneNumber);
            if (user is null)
                return NotFound("Пользователь не найден!");

            if (loginDto.Password != user.Password)
                return BadRequest("Неверный логин или пароль!");

            string jwtToken = GenerateToken(user);
            return Ok(new LoginResponse("Успешный вход в систему", jwtToken));

        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.UserData, user.CustomerId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.DateOfBirth, user.Birthday.Date.ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            if (registerDto.PhoneNumber.IsNullOrEmpty() ||
                registerDto.Password.IsNullOrEmpty() ||
                registerDto.Name.IsNullOrEmpty())
            {
                return BadRequest("Одно или несколько полей пустые!");
            }
                
            _accountRepository.RegisterUser(new User {
                CustomerId = Guid.NewGuid(),
                PhoneNumber = registerDto.PhoneNumber,
                Password = registerDto.Password,
                Name = registerDto.Name,
                Birthday = registerDto.Birthday,
                Email = registerDto.Email!
            });
            return Ok("Вы успешно зарегистрировались, далее нужно будет выполнить вход");
        }
    }
}
