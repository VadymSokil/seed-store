namespace SeedStore.Admin.Products.Models
{
    public class ResponseDiscountModel
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int ProductId { get; set; }
        public decimal DiscountPercent { get; set; }

    }
}
