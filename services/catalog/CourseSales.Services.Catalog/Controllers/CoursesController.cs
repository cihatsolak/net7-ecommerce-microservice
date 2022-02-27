namespace CourseSales.Services.Catalog.Controllers
{
    public sealed class CoursesController : BaseApiController
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAllAsync();
            return CreateActionResultInstance(courses);
        }

        [HttpGet("{id:minlength(1)}")]
        public async Task<IActionResult> GetById(string id)
        {
            var course = await _courseService.GetByIdAsync(id);
            return CreateActionResultInstance(course);
        }

        [HttpGet("{userId:minlength(1)}")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var courses = await _courseService.GetAllByUserIdAsync(userId);
            return CreateActionResultInstance(courses);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCourseRequstModel addCourseRequstModel)
        {
            var course = await _courseService.InsertAsync(addCourseRequstModel);
            return CreateActionResultInstance(course);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCourseRequestModel updateCourseRequstModel)
        {
            var result = await _courseService.UpdateAsync(updateCourseRequstModel);
            return CreateActionResultInstance(result);
        }

        [HttpDelete("{id:minlength(1)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _courseService.DeleteByIdAsync(id);
            return CreateActionResultInstance(result);
        }
    }
}
