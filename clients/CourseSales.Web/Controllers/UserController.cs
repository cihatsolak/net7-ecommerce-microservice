namespace CourseSales.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var userViewModel = await _userService.GetUserAsync();
            return View(userViewModel);
        }
    }
}
