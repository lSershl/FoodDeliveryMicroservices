namespace Ordering.Entities
{
    public class Order : IEntity
    {
        public Guid Id { get; set; }
        public required string Address { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
