namespace OrderDispatcher.CatalogService.API.Models
{
    public record BrandDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public int Order { get; init; }
    }
}
