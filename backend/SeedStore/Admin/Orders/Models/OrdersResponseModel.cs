namespace SeedStore.Admin.Orders.Models
{
    public class OrdersResponseModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string StatusCode { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DeliveryCode { get; set; } = string.Empty;
        public string PaymentCode { get; set; } = string.Empty;
    }
}
