namespace CourseSales.Services.Basket.Models.Basket
{
    public class BasketModel
    {
        public BasketModel()
        {
            BasketItemModel = new();
        }

        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public int? DiscountRate { get; set; }
        public List<BasketItemModel> BasketItemModel { get; set; }

        public decimal TotalPrice => BasketItemModel.Sum(item => item.Price * item.Quantity);
    }

    public class BasketItemModel
    {
        public int Quantity { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
    }
}
