namespace CourseSales.Services.Discount.Services
{
    public interface IDiscountService
    {
        Task<Response<List<DiscountResponseModel>>> GetAllAsync();

        Task<Response<DiscountResponseModel>> GetByIdAsync(int id);

        Task<Response<NoContentResponse>> SaveAsync(DiscountRequestModel discountRequestModel);

        Task<Response<NoContentResponse>> UpdateAsync(DiscountRequestModel discountRequestModel);

        Task<Response<NoContentResponse>> DeleteAsync(int id);

        Task<Response<DiscountResponseModel>> GetByCodeAndUserIdAsync(string code, string userId);
    }
}
