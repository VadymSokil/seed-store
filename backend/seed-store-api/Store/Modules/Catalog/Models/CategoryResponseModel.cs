namespace seed_store_api.Store.Modules.Catalog.Models
{
    public class CategoryResponseModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int? ViewOrder { get; set; }
    }
}
