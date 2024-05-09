using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using WebClient.Infrastructure;
using WebClient.Services;

namespace WebClient.Components.Pages.Customer
{
    public class OrdersBase : ComponentBase
    {
        [Inject]
        public required OrderService OrderService { get; set; }

        protected HubConnection? _connection;
        protected readonly string BaseHubConnectionUrl = "https://localhost:7202/ordering-service/order-status";
        protected Guid customerId = Guid.Parse("11a4eb82-356d-421d-9d97-2cbb73881111"); // hardcoded customerId, later will be passed as part of auth token
        protected IEnumerable<OrderDto>? ordersList;
        protected string currentOrderStatus = string.Empty;
        protected Guid? currentOrderId = Guid.Empty;

        protected override async Task OnInitializedAsync() // TODO: real time communication via SignalR for current order status updates
        {
            ordersList = await OrderService.GetCustomerOrders(customerId);
            if (ordersList is null || ordersList.Count() == 0)
                return;
            else
            {
                currentOrderId = ordersList.OrderBy(o => o.CreatedDate).First().Id;

                _connection = new HubConnectionBuilder()
                    .WithUrl(BaseHubConnectionUrl)
                    .WithKeepAliveInterval(TimeSpan.FromMilliseconds(1000))
                    .Build();

                _connection.On<Guid, string>("OrderStatusChanged", (orderId, status) =>
                {
                    currentOrderStatus = status;
                });

                await _connection.StartAsync();

                if (_connection.State == HubConnectionState.Connected)
                {
                    await _connection.SendAsync("GetOrderStatus", currentOrderId);
                }
            }
        }
    }
}
