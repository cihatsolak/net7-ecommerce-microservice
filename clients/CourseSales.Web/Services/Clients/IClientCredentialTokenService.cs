namespace CourseSales.Web.Services.Clients
{
    public interface IClientCredentialTokenService
    {
        Task<string> GetTokenAsync();
    }
}
