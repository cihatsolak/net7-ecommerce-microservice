namespace CourseSales.Web.Services.Identity
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignInAsync(SigninInput signinInput);

        Task<TokenResponse> GetAccessTokenByRefreshTokenAsync();

        Task RevokeRefreshTokenAsync();
    }
}
