using Catalog.Entities;

namespace Catalog.Infrastructure
{
    public static class Extensions
    {
        public static CatalogItemDto AsDto(this CatalogItem catalogItem)
        {
            return new CatalogItemDto(catalogItem.Id, catalogItem.Name, catalogItem.Description, catalogItem.PictureUrl, catalogItem.Price);
        }
    }
}
