using CourseSales.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseSales.IdentityServer.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var userExists = await _userManager.FindByEmailAsync(context.UserName);
            if (userExists is null)
            {
                context.Result.CustomResponse = GetPrepareCustomResponse();
                return;
            }

            var checkPassword = await _userManager.CheckPasswordAsync(userExists, context.Password);
            if (!checkPassword)
            {
                context.Result.CustomResponse = GetPrepareCustomResponse();
                return;
            }

            context.Result = new GrantValidationResult(userExists.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }

        private Dictionary<string, object> GetPrepareCustomResponse()
        {
            Dictionary<string, object> errors = new();
            errors.Add("errors", new List<string> { "Email veya şifreniz yanlış." });
            return errors;
        }

    }
}
