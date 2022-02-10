namespace CourseSales.Services.Catalog.Services
{
    internal interface ICourseService
    {
        Task<Response<NoContentResponse>> DeleteAsync(int id);
        Task<Response<List<CourseResponseModel>>> GetAllAsync();
        Task<Response<List<CourseResponseModel>>> GetAllByUserIdAsync(int userId);
        Task<Response<CourseResponseModel>> GetByIdAsync(int id);
        Task<Response<CourseResponseModel>> InsertAsync(AddCourseRequstModel addCourseRequstModel);
        Task<Response<NoContentResponse>> UpdateAsync(UpdateCourseRequestModel updateCourseRequestModel);
    }

    internal sealed class CourseManager : ICourseService
    {
        private readonly IMongoContext _mongoContext;
        private readonly IMapper _mapper;

        public CourseManager(
            IMapper mapper,
            IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
            _mapper = mapper;
        }

        public async Task<Response<List<CourseResponseModel>>> GetAllAsync()
        {
            var courses = await _mongoContext.Courses.Find(filter => true).ToListAsync();
            if (courses?.Any() ?? false)
            {
                return Response<List<CourseResponseModel>>.Fail("Kurs bulunamadı.", HttpStatusCode.NotFound);
            }

            courses.ForEach(async course =>
            {
                course.Category = await _mongoContext.Categories.Find(filter => filter.Id.Equals(course.CategoryId)).SingleOrDefaultAsync();
            });

            var coursesResponseModel = _mapper.Map<List<CourseResponseModel>>(courses);
            return Response<List<CourseResponseModel>>.Success(coursesResponseModel, HttpStatusCode.OK);
        }

        public async Task<Response<CourseResponseModel>> GetByIdAsync(int id)
        {
            if (0 >= id)
                throw new ArgumentNullException(nameof(id));

            var course = await _mongoContext.Courses.Find(filter => filter.Id.Equals(id)).SingleOrDefaultAsync();
            if (course is null)
            {
                return Response<CourseResponseModel>.Fail("Kurs bulunamadı.", HttpStatusCode.NotFound);
            }

            course.Category = await _mongoContext.Categories.Find(filter => filter.Id.Equals(course.CategoryId)).SingleOrDefaultAsync();

            var courseResponseModel = _mapper.Map<CourseResponseModel>(course);
            return Response<CourseResponseModel>.Success(courseResponseModel, HttpStatusCode.OK);
        }

        public async Task<Response<List<CourseResponseModel>>> GetAllByUserIdAsync(int userId)
        {
            var courses = await _mongoContext.Courses.Find(filter => filter.UserId.Equals(userId)).ToListAsync();
            if (courses?.Any() ?? false)
            {
                return Response<List<CourseResponseModel>>.Fail("Kurs bulunamadı.", HttpStatusCode.NotFound);
            }

            courses.ForEach(async course =>
            {
                course.Category = await _mongoContext.Categories.Find(filter => filter.Id.Equals(course.CategoryId)).SingleOrDefaultAsync();
            });

            var coursesResponseModel = _mapper.Map<List<CourseResponseModel>>(courses);
            return Response<List<CourseResponseModel>>.Success(coursesResponseModel, HttpStatusCode.OK);
        }

        public async Task<Response<CourseResponseModel>> InsertAsync(AddCourseRequstModel addCourseRequstModel)
        {
            var course = _mapper.Map<Course>(addCourseRequstModel);
            await _mongoContext.Courses.InsertOneAsync(course);

            var courseResponseModel = _mapper.Map<CourseResponseModel>(course);
            return Response<CourseResponseModel>.Success(courseResponseModel, HttpStatusCode.OK);
        }

        public async Task<Response<NoContentResponse>> UpdateAsync(UpdateCourseRequestModel updateCourseRequestModel)
        {
            var course = _mapper.Map<Course>(updateCourseRequestModel);
            course = await _mongoContext.Courses.FindOneAndReplaceAsync(p => p.Id.Equals(updateCourseRequestModel.Id), course);
            if (course is null)
            {
                return Response<NoContentResponse>.Fail("Kurs bulunamadı.", HttpStatusCode.NotFound);
            }

            return Response<NoContentResponse>.Success(HttpStatusCode.OK);
        }

        public async Task<Response<NoContentResponse>> DeleteAsync(int id)
        {
            var deleteResult = await _mongoContext.Courses.DeleteOneAsync(filter => filter.Id.Equals(id));
            if (0 >= deleteResult.DeletedCount)
            {
                return Response<NoContentResponse>.Fail("Kurs bulunamadı.", HttpStatusCode.NotFound);
            }

            return Response<NoContentResponse>.Success(HttpStatusCode.OK);
        }
    }
}
