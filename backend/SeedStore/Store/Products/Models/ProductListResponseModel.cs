namespace SeedStore.Store.Products.Models
{
    public class ProductListResponseModel
    {
        public List<ProductCardModel> Items { get; set; } = [];
        public int TotalCount { get; set; }
    }
}
