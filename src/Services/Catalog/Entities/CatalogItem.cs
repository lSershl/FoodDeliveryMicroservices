namespace Catalog.Entities
{
    public class CatalogItem : IEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string ImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}
