namespace CourseSales.Web.Services.Discounts
{
    public interface IDiscountService
    {
        Task<DiscountViewModel> GetDiscountAsync(string discountCode);
    }
}
