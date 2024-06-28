using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(builderContext =>
    {
        builderContext.AddRequestTransform(async transformContext =>
        {
            var accessToken = await transformContext.HttpContext.GetTokenAsync("access_token");
            if (accessToken != null)
            {
                transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        });
    });

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Jwt:Issuer"];
        options.Audience = builder.Configuration["Jwt:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateLifetime = true // This checks the expiry
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("customPolicy", policy =>
        policy.RequireAuthenticatedUser()
        );
});

var app = builder.Build();

app.UseRouting();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

// Custom JWT validation middleware
//app.Use(async (context, next) =>
//{
//    if (
//        context.Request.Path.Value!.Contains(builder.Configuration["AnonymousRequests:Catalog"]!) ||
//        context.Request.Path.Value.Contains(builder.Configuration["AnonymousRequests:Identity"]!))
//    {
//        return;
//    }
//    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
//    if (string.IsNullOrEmpty(token))
//    {
//        context.Response.StatusCode = 401;
//        await context.Response.WriteAsync("Authorization header missing.");
//        return;
//    }

//    try
//    {
//        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
//        var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);

//        // Determine the API (audience) based on the request, and set issuer and audience values accordingly
//        var validIssuer = builder.Configuration["Jwt:Issuer"];
//        var validAudience = builder.Configuration["Jwt:Audience"];

//        var validationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidIssuer = validIssuer,
//            ValidAudience = validAudience,
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
//            ValidateLifetime = true // This checks the expiry
//        };

//        // This will throw if invalid
//        jwtSecurityTokenHandler.ValidateToken(token, validationParameters, out _);
//    }
//    catch
//    {
//        context.Response.StatusCode = 401;
//        await context.Response.WriteAsync("Invalid token.");
//        return;
//    }

//    await next.Invoke();
//});

app.MapReverseProxy();

app.Run();
