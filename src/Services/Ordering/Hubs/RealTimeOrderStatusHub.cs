using Microsoft.AspNetCore.SignalR;
using Ordering.Entities;

namespace Ordering.Hubs
{
    public class RealTimeOrderStatusHub(IRepository<Order> repository) : Hub
    {
        private readonly IRepository<Order> _repository = repository;

        public async Task GetOrderStatus(Guid orderId)
        {
            var order = await _repository.GetAsync(orderId);
            await Clients.Caller.SendAsync("OrderStatusUpdate", order.Status);
        }
    }
}
