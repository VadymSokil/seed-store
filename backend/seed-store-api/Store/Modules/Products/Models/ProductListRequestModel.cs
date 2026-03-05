namespace seed_store_api.Store.Modules.Products.Models
{
    public class ProductListRequestModel
    {
        public int? CategoryId { get; set; }
        public string? CategorySlug { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public bool? SortByPriceAsc { get; set; }
        public bool? SortByPriceDesc { get; set; }
        public bool? HasDiscount { get; set; }
        public bool? InStock { get; set; }
        public List<ActiveFilterModel>? ActiveFilters { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
