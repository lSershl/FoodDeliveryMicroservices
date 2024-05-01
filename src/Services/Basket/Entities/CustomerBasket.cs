namespace Basket.Entities
{
    public class CustomerBasket
    {
        public Guid CustomerId { get; set; }

        public List<BasketItem> Items { get; set; } = [];

        public CustomerBasket() { }

        public CustomerBasket(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}
