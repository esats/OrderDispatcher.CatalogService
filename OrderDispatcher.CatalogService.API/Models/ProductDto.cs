namespace OrderDispatcher.CatalogService.API.Models
{
    public record ProductDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string SKU { get; init; }
        public decimal Price { get; init; }
        public int Stock { get; init; }
        public int BrandId { get; init; }
        public int CategoryId { get; init; }
        public int ImageMasterId { get; init; }
        public int Order { get; init; }
    }
}
