using System.ComponentModel.DataAnnotations;

namespace seed_store_api.Store.Modules.Orders.Models
{
    public class PaymentCallbackModel
    {
        [Required]
        public string Data { get; set; } = string.Empty;

        [Required]
        public string Signature { get; set; } = string.Empty;
    }
}
