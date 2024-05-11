using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using WebClient.Infrastructure;
using WebClient.Models;
using WebClient.Services;

namespace WebClient.Components.Pages.Customer
{
    public class OrdersBase : ComponentBase
    {
        [Inject]
        public required OrderService OrderService { get; set; }
        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        protected HubConnection? _connection;
        protected readonly string BaseHubConnectionUrl = "https://localhost:7202/order-status/get-status";
        protected Guid customerId = Guid.Parse("11a4eb82-356d-421d-9d97-2cbb73881111"); // hardcoded customerId, later will be passed as part of auth token
        protected IEnumerable<OrderDto>? ordersDtoList;
        protected List<CustomerOrder> customerOrders = new();

        protected override async Task OnInitializedAsync() // TODO: real time communication via SignalR for current order status updates
        {
            try
            {
                ordersDtoList = await OrderService.GetCustomerOrders(customerId);
                if (ordersDtoList is null || ordersDtoList.Count() == 0)
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
                    customerOrders = customerOrders.OrderByDescending(o => o.CreatedDate).ToList();
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
