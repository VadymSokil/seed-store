using System.ComponentModel.DataAnnotations;

namespace SeedStore.Store.Account.Models
{
    public class ChangePhoneModel
    {
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
    }
}
