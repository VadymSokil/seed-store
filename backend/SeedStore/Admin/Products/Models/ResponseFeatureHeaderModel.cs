namespace SeedStore.Admin.Products.Models
{
    public class ResponseFeatureHeaderModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? ViewOrder { get; set; }
    }
}
