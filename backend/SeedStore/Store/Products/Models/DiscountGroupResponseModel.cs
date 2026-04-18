namespace SeedStore.Store.Products.Models
{
    public class DiscountGroupResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<DiscountResponseModel> Products { get; set; } = [];
    }
}
