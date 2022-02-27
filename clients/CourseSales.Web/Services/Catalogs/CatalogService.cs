namespace CourseSales.Web.Services.Catalogs
{
    public sealed class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {
            var httpResponseMessage = await _httpClient.PostAsJsonAsync("courses/add", courseCreateInput);
            return httpResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var httpResponseMessage = await _httpClient.DeleteAsync($"courses/delete/{courseId}");
            return httpResponseMessage.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            var httpResponseMessage = await _httpClient.GetAsync("categories/getall");
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            var responseCategoriesViewModel = await httpResponseMessage.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();
            return responseCategoriesViewModel.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseAsync()
        {
            var httpResponseMessage = await _httpClient.GetAsync("courses/getall");
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            var responseCoursesViewModel = await httpResponseMessage.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            return responseCoursesViewModel.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            var httpResponseMessage = await _httpClient.GetAsync($"courses/getallbyuserid/{userId}");
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            var responseCourseViewModel = await httpResponseMessage.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            return responseCourseViewModel.Data;
        }

        public async Task<CourseViewModel> GetByCourseId(string courseId)
        {
            var httpResponseMessage = await _httpClient.GetAsync($"courses/getbyid/{courseId}");
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            var responseCourseViewModel = await httpResponseMessage.Content.ReadFromJsonAsync<Response<CourseViewModel>>();
            return responseCourseViewModel.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var httpResponseMessage = await _httpClient.PutAsJsonAsync("courses/update", courseUpdateInput);
            return httpResponseMessage.IsSuccessStatusCode;
        }
    }
}
