namespace CourseSales.Services.Catalog.Controllers
{
    public sealed class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return CreateActionResultInstance(categories);
        }

        [HttpGet("{id:minlength(1)}")]
        public async Task<IActionResult> GetById(string id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return CreateActionResultInstance(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddCategoryRequestModel addCategoryRequestModel)
        {
            var result = await _categoryService.InsertAsync(addCategoryRequestModel);
            return CreateActionResultInstance(result);
        }
    }
}
