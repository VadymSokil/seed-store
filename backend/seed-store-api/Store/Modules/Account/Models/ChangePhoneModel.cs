using System.ComponentModel.DataAnnotations;

namespace seed_store_api.Store.Modules.Account.Models
{
    public class ChangePhoneModel
    {
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
    }
}
