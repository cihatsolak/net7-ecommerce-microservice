namespace CourseSales.Services.Basket.Models.Basket
{
    public class BasketResponseModel
    {
        public BasketResponseModel()
        {
            BasketItemsResponseModel = new();
        }

        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public List<BasketItemResponseModel> BasketItemsResponseModel { get; set; }

        public decimal TotalPrice => BasketItemsResponseModel.Sum(item => item.Price * item.Quantity);
    }

    public class BasketItemResponseModel
    {
        public int Quantity { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
    }
}
