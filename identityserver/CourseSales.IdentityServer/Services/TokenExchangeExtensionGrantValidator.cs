using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace CourseSales.IdentityServer.Services
{
    public class TokenExchangeExtensionGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => "urn:ietf:params:oauth:grant-type:token-exchange";
        private readonly ITokenValidator _tokenValidator;

        public TokenExchangeExtensionGrantValidator(ITokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            string requestRaw = context.Request.Raw.ToString();

            string accessToken = context.Request.Raw.Get("subject_token");
            if (string.IsNullOrEmpty(accessToken))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "token missing");
                return;
            }

            var tokenValidateResult = await _tokenValidator.ValidateAccessTokenAsync(accessToken);
            if (tokenValidateResult.IsError)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "token invalid");
                return;
            }

            var subjectClaim = tokenValidateResult.Claims.FirstOrDefault(c => c.Type == "sub");
            if (subjectClaim is null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "token must contain sub value");
                return;
            }

            context.Result = new GrantValidationResult(subjectClaim.Value, "access_token", tokenValidateResult.Claims);
            return;
        }
    }
}
