namespace SeedStore.Store.Products.Models
{
    public class ProductTopResponseModel
    {
        public List<ProductCardModel> Newest { get; set; }
        public List<ProductCardModel> Popular { get; set; }
        public List<ProductCardModel> MostDiscussed { get; set; }
    }
}
