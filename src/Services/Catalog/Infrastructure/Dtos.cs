using System.ComponentModel.DataAnnotations;

namespace Catalog.Infrastructure
{
    public record CatalogItemDto(Guid Id, string Name, string Description, string PictureUrl, decimal Price);
    public record CreateCatalogItemDto([Required] string Name, [Required] string Description, [Required] string PictureUrl, [Range(0, 10000)] decimal Price);
    public record UpdateCatalogItemDto([Required] string Name, [Required] string Description, [Required] string PictureUrl, [Range(0, 10000)] decimal Price);
}
