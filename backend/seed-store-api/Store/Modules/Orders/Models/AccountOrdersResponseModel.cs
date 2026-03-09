namespace seed_store_api.Store.Modules.Orders.Models
{
    public class AccountOrdersResponseModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public string? Comment { get; set; }
        public string DeliveryCode { get; set; } = string.Empty;
        public string PaymentCode { get; set; } = string.Empty;
        public string? PostalOfficeNumber { get; set; }
        public string? TrackingNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Region { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? Settlement { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public string? ApartmentNumber { get; set; }
        public DateTime? PaidAt { get; set; }
        public List<OrderItemsModel> Items { get; set; } = [];
        public List<OrderTransactionsModel> Transactions { get; set; } = [];
    }
}