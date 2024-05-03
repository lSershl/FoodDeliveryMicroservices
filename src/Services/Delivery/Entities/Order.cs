using static Delivery.Infrastructure.Dtos;

namespace Delivery.Entities
{
    public class Order : IEntity
    {
        public Guid Id { get; set; }
        public required string CustomerName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
        public required string DeliveryTime { get; set; }
        public required List<BasketItemDto> Items { get; set; }

        public required string Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
