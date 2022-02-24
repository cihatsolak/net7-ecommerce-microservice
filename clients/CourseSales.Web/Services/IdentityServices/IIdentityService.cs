namespace CourseSales.Web.Services.IdentityServices
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignInAsync(SigninInput signinInput);

        Task<TokenResponse> GetAccessTokenByRefreshTokenAsync();

        Task RevokeRefreshTokenAsync();
    }
}
