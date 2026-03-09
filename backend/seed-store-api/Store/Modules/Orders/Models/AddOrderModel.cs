namespace seed_store_api.Store.Modules.Orders.Models
{
    public class AddOrderModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string DeliveryCode { get; set; } = string.Empty;
        public string PaymentCode { get; set; } = string.Empty;
        public string? PostalOfficeNumber { get; set; }
        public string? Region { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? Settlement { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public string? ApartmentNumber { get; set; }
        public string? Comment { get; set; }
        public List<AddOrderItemModel> Items { get; set; } = [];
    }
}
