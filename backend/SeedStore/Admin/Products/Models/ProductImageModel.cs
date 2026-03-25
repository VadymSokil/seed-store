namespace SeedStore.Admin.Products.Models
{
    public class ProductImageModel
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public int? ViewOrder { get; set; }
    }
}
