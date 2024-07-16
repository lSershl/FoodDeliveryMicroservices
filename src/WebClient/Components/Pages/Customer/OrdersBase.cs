using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using WebClient.Infrastructure;
using WebClient.Models;
using WebClient.Services;

namespace WebClient.Components.Pages.Customer
{
    public class OrdersBase : ComponentBase
    {
        [CascadingParameter]
        protected Task<AuthenticationState>? AuthStateTask { get; init; }
        [Inject]
        public required OrderService OrderService { get; init; }
        [Inject]
        public required ILocalStorageService LocalStorage { get; init; }
        [Inject]
        public required NavigationManager NavigationManager { get; init; }

        protected HubConnection? _connection;
        protected readonly string BaseHubConnectionUrl = "https://localhost:7202/order-status/get-status";
        
        protected Guid customerId = Guid.Empty;
        protected IEnumerable<OrderDto>? ordersDtoList;
        protected List<CustomerOrder> customerOrders = [];

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (AuthStateTask is not null)
                {
                    var authState = await AuthStateTask;
                    var user = authState.User;
                    if (user.Identity!.IsAuthenticated)
                        customerId = Guid.Parse(user.Claims.First(x => x.Type.Contains("userdata"))!.Value);
                }

                var token = await LocalStorage.GetItemAsync<string>("JWTToken");
                ordersDtoList = await OrderService.GetCustomerOrders(customerId, token!);
                if (ordersDtoList is null || !ordersDtoList.Any())
                    return;
                else
                {
                    foreach (var orderDto in ordersDtoList)
                    {
                        customerOrders.Add(new CustomerOrder
                        {
                            Id = orderDto.Id,
                            Address = orderDto.Address,
                            DeliveryTime = orderDto.DeliveryTime,
                            Items = orderDto.Items,
                            Status = orderDto.Status,
                            CreatedDate = orderDto.CreatedDate
                        });
                    }
                    customerOrders = [.. customerOrders.OrderByDescending(o => o.CreatedDate)];
                    var currentOrder = customerOrders.First();
                    if (currentOrder.Status == "Доставлен")
                        return;

                    _connection = new HubConnectionBuilder()
                        .WithUrl(BaseHubConnectionUrl)
                        .Build();

                    _connection.On<string>("OrderStatusUpdate", status =>
                    {
                        customerOrders.First(o => o.Id == currentOrder.Id).Status = status;
                        InvokeAsync(StateHasChanged);
                    });

                    await _connection.StartAsync();

                    while (_connection.State == HubConnectionState.Connected)
                    {
                        await _connection!.SendAsync("GetOrderStatus", currentOrder.Id);
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
