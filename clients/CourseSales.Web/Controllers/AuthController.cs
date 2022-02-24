namespace CourseSales.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SigninInput signinInput)
        {
            if (!ModelState.IsValid)
                return View(signinInput);

            var response = await _identityService.SignInAsync(signinInput);
            if (!response.IsSuccessful)
            {
                response.Errors.ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error);
                });

                return View(signinInput);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
