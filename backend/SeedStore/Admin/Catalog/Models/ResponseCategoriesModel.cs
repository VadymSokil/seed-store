namespace SeedStore.Admin.Catalog.Models
{
    public class ResponseCategoriesModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int? ViewOrder { get; set; }
    }
}
