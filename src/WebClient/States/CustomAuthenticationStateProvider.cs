using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebClient.Infrastructure;

namespace WebClient.States
{
    public class CustomAuthenticationStateProvider(ILocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());

        private readonly ILocalStorageService? localStorage = localStorageService;

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorage!.GetItemAsync<string>("JWTToken");
            try
            {
                if (string.IsNullOrEmpty(token))
                    return await Task.FromResult(new AuthenticationState(anonymous));

                var getUserClaims = DecryptToken(token);
                if (getUserClaims is null)
                    return await Task.FromResult(new AuthenticationState(anonymous));

                var claimsPrincipal = SetClaimPrincipal(getUserClaims);
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
        }

        public async void UpdateAuthenticationState(string jwtToken)
        {
            var claimsPrincipal = new ClaimsPrincipal();

            var getUserClaims = DecryptToken(jwtToken);
            claimsPrincipal = SetClaimPrincipal(getUserClaims);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        private static ClaimsPrincipal SetClaimPrincipal(CustomUserClaims claims)
        {
            if (claims.Name is null) return new ClaimsPrincipal();
            return new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new (ClaimTypes.UserData, claims.CustomerId),
                    new (ClaimTypes.Name, claims.Name),
                    new (ClaimTypes.MobilePhone, claims.PhoneNumber),
                    new (ClaimTypes.DateOfBirth, claims.Birthday)
                }, "JwtAuth"));
        }

        private static CustomUserClaims DecryptToken(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken))
                return new CustomUserClaims();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var customerId = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.UserData);
            var name = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name);
            var phoneNumber = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.MobilePhone);
            var birthday = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.DateOfBirth);
            return new CustomUserClaims(customerId!.Value, name!.Value, phoneNumber!.Value, birthday!.Value);
        }
    }
}
