using System.ComponentModel.DataAnnotations;

namespace seed_store_api.Store.Modules.Orders.Models
{
    public class AddOrderItemModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
