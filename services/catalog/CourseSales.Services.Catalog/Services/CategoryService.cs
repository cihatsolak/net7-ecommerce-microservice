namespace CourseSales.Services.Catalog.Services
{
    internal interface ICategoryService
    {
        Task<Response<List<CategoryResponseModel>>> GetAllAsync();
        Task<Response<CategoryResponseModel>> GetById(int id);
        Task<Response<CategoryResponseModel>> InsertAsync(Category category);
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

        public async Task<Response<CategoryResponseModel>> GetById(int id)
        {
            if (0 >= id)
                throw new ArgumentNullException(nameof(id));

            var category = await _mongoContext.Categories.Find(filter => filter.Id.Equals(id)).SingleOrDefaultAsync();
            if (category is null)
            {
                return Response<CategoryResponseModel>.Fail("Category bulunamadı.", HttpStatusCode.NotFound);
            }

            var categoryResponseModel = _mapper.Map<CategoryResponseModel>(category);
            return Response<CategoryResponseModel>.Success(categoryResponseModel, HttpStatusCode.OK);
        }

        public async Task<Response<CategoryResponseModel>> InsertAsync(Category category)
        {
            await _mongoContext.Categories.InsertOneAsync(category);
            var categoryResponseModel = _mapper.Map<CategoryResponseModel>(category);
            return Response<CategoryResponseModel>.Success(categoryResponseModel, HttpStatusCode.OK);
        }
    }
}
