using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using WebClient.Components;
using WebClient.Services;
using WebClient.States;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.ConfigureHttpClientDefaults(http =>
{
    http.AddServiceDiscovery();
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//builder.Services.AddScoped(http => new HttpClient
//{
//    BaseAddress = new Uri(builder.Configuration.GetSection("GatewayAddress").Value!)
//});

//builder.Services.AddScoped(sp => new HttpClient
//{
//    BaseAddress = new("https://yarp-gateway")
//});

builder.Services.AddHttpClient("YARPGateway", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("GatewayAddress").Value!);
    //client.BaseAddress = new Uri("https://yarp-gateway");
});


builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<CatalogService>();
builder.Services.AddScoped<BasketService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<PaymentCardService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
