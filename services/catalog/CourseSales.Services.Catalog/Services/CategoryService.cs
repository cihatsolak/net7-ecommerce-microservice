namespace CourseSales.Services.Catalog.Services
{
    internal interface ICategoryService
    {
        Task<Response<List<CategoryResponseModel>>> GetAllAsync();
        Task<Response<CategoryResponseModel>> GetByIdAsync(string id);
        Task<Response<CategoryResponseModel>> InsertAsync(AddCategoryRequestModel addCategoryRequestModel);
    }

    internal sealed class CategoryManager : ICategoryService
    {
        private readonly IMongoContext _mongoContext;
        private readonly IMapper _mapper;

        public CategoryManager(
            IMapper mapper,
            IMongoContext mongoContext)
        {
            _mapper = mapper;
            _mongoContext = mongoContext;
        }

        public async Task<Response<List<CategoryResponseModel>>> GetAllAsync()
        {
            var categories = await _mongoContext.Categories.Find(filter => true).ToListAsync();
            if (categories?.Any() ?? false)
            {
                return Response<List<CategoryResponseModel>>.Fail("Category bulunamadı.", HttpStatusCode.NotFound);
            }

            var categoriesResponseModel = _mapper.Map<List<CategoryResponseModel>>(categories);
            return Response<List<CategoryResponseModel>>.Success(categoriesResponseModel, HttpStatusCode.OK);
        }

        public async Task<Response<CategoryResponseModel>> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            var category = await _mongoContext.Categories.Find(filter => filter.Id.Equals(id)).SingleOrDefaultAsync();
            if (category is null)
            {
                return Response<CategoryResponseModel>.Fail("Category bulunamadı.", HttpStatusCode.NotFound);
            }

            var categoryResponseModel = _mapper.Map<CategoryResponseModel>(category);
            return Response<CategoryResponseModel>.Success(categoryResponseModel, HttpStatusCode.OK);
        }

        public async Task<Response<CategoryResponseModel>> InsertAsync(AddCategoryRequestModel addCategoryRequestModel)
        {
            var category = _mapper.Map<Category>(addCategoryRequestModel);

            await _mongoContext.Categories.InsertOneAsync(category);
            var categoryResponseModel = _mapper.Map<CategoryResponseModel>(category);
            return Response<CategoryResponseModel>.Success(categoryResponseModel, HttpStatusCode.OK);
        }
    }
}
