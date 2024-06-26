﻿using WebClient.Infrastructure;

namespace WebClient.Models
{
    public class CustomerOrder
    {
        public Guid Id { get; set; }
        public required string Address { get; set; }
        public required string DeliveryTime { get; set; }
        public required List<BasketItemDto> Items { get; set; }

        public required string Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
