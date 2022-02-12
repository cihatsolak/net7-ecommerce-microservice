using CourseSales.IdentityServer.Models;
using CourseSales.Shared.Controllers;
using CourseSales.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CourseSales.IdentityServer.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpRequestModel signUpRequestModel)
        {
            ApplicationUser user = new()
            {
                UserName = signUpRequestModel.Username,
                Email = signUpRequestModel.Email,
                City = signUpRequestModel.City
            };

            var identityResult = await _userManager.CreateAsync(user, signUpRequestModel.Password);
            if (!identityResult.Succeeded)
            {
                return BadRequest(Response<NoContentResponse>.Fail(identityResult.Errors.Select(p => p.Description).ToList(), HttpStatusCode.BadRequest));
            }

            return Created(nameof(SignUp), null);
        }
    }
}
