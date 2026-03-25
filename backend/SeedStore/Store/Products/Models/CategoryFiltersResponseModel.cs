namespace SeedStore.Store.Products.Models
{
    public class CategoryFiltersResponseModel
    {
        public List<FeatureItemModel> Features { get; set; }
        public List<FeatureHeaderItemModel> Headers { get; set; }
        public List<FilterValueItemModel> FilterValues { get; set; }
    }
}
