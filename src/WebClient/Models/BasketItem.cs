﻿namespace WebClient.Models
{
    public class BasketItem
    {
        public Guid ProductId { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public required string PictureUrl { get; set; }
    }
}
