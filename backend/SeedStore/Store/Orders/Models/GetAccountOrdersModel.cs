using System.ComponentModel.DataAnnotations;

namespace SeedStore.Store.Orders.Models
{
    public class GetAccountOrdersModel
    {
        public int Page {  get; set; }
        [Range(1, 50)]
        public int PageSize { get; set; }
    }
}
