using Microsoft.AspNetCore.Mvc;

namespace CourseSales.Web.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
