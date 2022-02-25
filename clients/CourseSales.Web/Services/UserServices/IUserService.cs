namespace CourseSales.Web.Services.UserServices
{
    public interface IUserService
    {
        Task<UserViewModel> GetUserAsync();
    }
}
