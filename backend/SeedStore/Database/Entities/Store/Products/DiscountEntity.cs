namespace SeedStore.Database.Entities.Store.Products
{
    public class DiscountEntity
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int ProductId { get; set; }
        public decimal DiscountPercent { get; set; }
    }
}
