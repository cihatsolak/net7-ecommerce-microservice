namespace CourseSales.Services.Payment.Models
{
    public class OrderRequestModel
    {
        public OrderRequestModel()
        {
            OrderItems = new List<OrderItemRequestModel>();
        }

        public string BuyerId { get; set; }
        public List<OrderItemRequestModel> OrderItems { get; set; }
        public AddresRequestModel Address { get; set; }
    }

    public class AddresRequestModel
    {
        public string Province { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Line { get; set; }
    }

    public class OrderItemRequestModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
    }
}
