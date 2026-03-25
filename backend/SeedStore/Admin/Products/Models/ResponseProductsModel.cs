namespace SeedStore.Admin.Products.Models
{
    public class ResponseProductsModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Article { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
