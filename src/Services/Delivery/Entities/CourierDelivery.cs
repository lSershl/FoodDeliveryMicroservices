namespace Delivery.Entities
{
    public class CourierDelivery : IEntity
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid CourierId { get; set; }
        public string? Status { get; set; }
        public DateTimeOffset AcquiredDate { get; set; }
    }
}
