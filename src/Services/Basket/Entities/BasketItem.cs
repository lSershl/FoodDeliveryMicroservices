﻿namespace Basket.Entities
{
    public class BasketItem : IEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public required string ImageUrl { get; set; }
    }
}