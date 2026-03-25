using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Orders.Models
{
    public class UpdateOrderModel
    {
        [MaxLength(500)]
        public string? Comment { get; set; }
        [Required]
        [MaxLength(50)]
        public string DeliveryCode { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string PaymentCode { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? PostalOfficeNumber { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? MiddleName { get; set; }
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? Region { get; set; }
        [MaxLength(100)]
        public string? District { get; set; }
        [MaxLength(100)]
        public string? City { get; set; }
        [MaxLength(100)]
        public string? Settlement { get; set; }
        [MaxLength(200)]
        public string? Street { get; set; }
        [MaxLength(20)]
        public string? HouseNumber { get; set; }
        [MaxLength(20)]
        public string? ApartmentNumber { get; set; }
        public List<UpdateOrderItemModel> Items { get; set; } = [];
    }
}
