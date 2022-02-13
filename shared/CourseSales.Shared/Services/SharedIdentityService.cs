namespace CourseSales.Shared.Services
{
    public interface ISharedIdentityService
    {
        public string UserId { get;  }
    }

    public class SharedIdentityService : ISharedIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(claim => claim.Type.Equals("sub"))?.Value;
    }
}
