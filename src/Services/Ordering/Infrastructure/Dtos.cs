using System.ComponentModel.DataAnnotations;

namespace Ordering.Infrastructure
{
    public record OrderDto(Guid Id, Guid CustomerId, string CustomerName, string PhoneNumber, string Address, string DeliveryTime, List<BasketItemDto> Items, string Status, DateTimeOffset CreatedDate);
    public record CreateOrderDto([Required] string CustomerName, [Required] string PhoneNumber, [Required] string Address, [Required] string DeliveryTime, List<BasketItemDto> Items);
    public record UpdateOrderDto([Required] string PhoneNumber, [Required] string Address, [Required] string DeliveryTime, List<BasketItemDto> Items);
    public record BasketItemDto(Guid ProductId, string ProductName, decimal Price, [Range(0, 100)] int Quantity, string PictureUrl);
}
