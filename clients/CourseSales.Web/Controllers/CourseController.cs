namespace CourseSales.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public CoursesController(
            ICatalogService catalogService, 
            ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var coursesViewModel = await _catalogService.GetAllCourseByUserIdAsync(_sharedIdentityService.UserId);
            return View(coursesViewModel);
        }

        //public async Task<IActionResult> Create()
        //{
        //    var categories = await _catalogService.GetAllCategoryAsync();

        //    ViewBag.categoryList = new SelectList(categories, "Id", "Name");

        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
        //{
        //    var categories = await _catalogService.GetAllCategoryAsync();
        //    ViewBag.categoryList = new SelectList(categories, "Id", "Name");
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    courseCreateInput.UserId = _sharedIdentityService.GetUserId;

        //    await _catalogService.CreateCourseAsync(courseCreateInput);

        //    return RedirectToAction(nameof(Index));
        //}

        //public async Task<IActionResult> Update(string id)
        //{
        //    var course = await _catalogService.GetByCourseId(id);
        //    var categories = await _catalogService.GetAllCategoryAsync();

        //    if (course == null)
        //    {
        //        //mesaj göster
        //        RedirectToAction(nameof(Index));
        //    }
        //    ViewBag.categoryList = new SelectList(categories, "Id", "Name", course.Id);
        //    CourseUpdateInput courseUpdateInput = new()
        //    {
        //        Id = course.Id,
        //        Name = course.Name,
        //        Description = course.Description,
        //        Price = course.Price,
        //        Feature = course.Feature,
        //        CategoryId = course.CategoryId,
        //        UserId = course.UserId,
        //        Picture = course.Picture
        //    };

        //    return View(courseUpdateInput);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Update(CourseUpdateInput courseUpdateInput)
        //{
        //    var categories = await _catalogService.GetAllCategoryAsync();
        //    ViewBag.categoryList = new SelectList(categories, "Id", "Name", courseUpdateInput.Id);
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    await _catalogService.UpdateCourseAsync(courseUpdateInput);

        //    return RedirectToAction(nameof(Index));
        //}

        //public async Task<IActionResult> Delete(string id)
        //{
        //    await _catalogService.DeleteCourseAsync(id);

        //    return RedirectToAction(nameof(Index));
        //}
    }
}
