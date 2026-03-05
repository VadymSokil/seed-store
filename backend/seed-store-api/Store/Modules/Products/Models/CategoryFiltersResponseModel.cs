namespace seed_store_api.Store.Modules.Products.Models
{
    public class CategoryFiltersResponseModel
    {
        public List<FeatureItemModel> Features { get; set; }
        public List<FeatureHeaderItemModel> Headers { get; set; }
        public List<FilterValueItemModel> FilterValues { get; set; }
    }
}
