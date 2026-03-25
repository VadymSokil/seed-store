namespace SeedStore.Database.Entities.Store.Products
{
    public class ProductImageEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; } = string.Empty;
        public int? ViewOrder { get; set; }
    }
}
