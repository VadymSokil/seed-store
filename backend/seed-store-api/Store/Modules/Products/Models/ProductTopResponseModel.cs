namespace seed_store_api.Store.Modules.Products.Models
{
    public class ProductTopResponseModel
    {
        public List<ProductCardModel> Newest { get; set; }
        public List<ProductCardModel> Popular { get; set; }
        public List<ProductCardModel> MostDiscussed { get; set; }
    }
}
