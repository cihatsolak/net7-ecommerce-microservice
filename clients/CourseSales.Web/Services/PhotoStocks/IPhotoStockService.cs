namespace CourseSales.Web.Services.PhotoStocks
{
    public interface IPhotoStockService
    {
        Task<PhotoViewModel> UploadPhotoAsync(IFormFile photo);
        Task<bool> DeletePhotoAsync(string photoUrl);
    }
}
