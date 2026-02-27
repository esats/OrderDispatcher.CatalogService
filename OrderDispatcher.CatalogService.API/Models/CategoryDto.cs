namespace OrderDispatcher.CatalogService.API.Models
{
    public record CategoryDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public int ParentId { get; init; }
        public int Order { get; init; }
        public List<CategoryDto>? ParentCategories { get; set; }
    }
}
