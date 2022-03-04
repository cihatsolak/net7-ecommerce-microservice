namespace CourseSales.Web.Services.Users
{
    public interface IUserService
    {
        Task<UserViewModel> GetUserAsync();
    }
}
