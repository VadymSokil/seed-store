using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Orders.Models
{
    public class UpdateOrderItemModel
    {
        public int ItemId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
