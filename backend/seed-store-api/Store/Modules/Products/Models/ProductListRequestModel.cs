using System.ComponentModel.DataAnnotations;

namespace seed_store_api.Store.Modules.Products.Models
{
    public class ProductListRequestModel
    {
        public int? CategoryId { get; set; }

        [MaxLength(200)]
        public string? CategorySlug { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? PriceFrom { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? PriceTo { get; set; }

        public bool? SortByPriceAsc { get; set; }
        public bool? SortByPriceDesc { get; set; }
        public bool? HasDiscount { get; set; }
        public bool? InStock { get; set; }
        public List<ActiveFilterModel>? ActiveFilters { get; set; }

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 12;
    }
}
