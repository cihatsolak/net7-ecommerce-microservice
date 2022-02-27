namespace CourseSales.Services.Catalog.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CourseMaps();
            CategoryMaps();
            FeatureMaps();
        }

        private void CourseMaps()
        {
            CreateMap<Course, CourseResponseModel>();
            CreateMap<Category, CategoryResponseModel>();
            CreateMap<Feature, FeatureResponseModel>();

            CreateMap<AddCourseRequstModel, Course>();
            CreateMap<UpdateCourseRequstModel, Course>();
        }

        private void CategoryMaps()
        {
            CreateMap<AddCategoryRequestModel, Category>();
        }

        private void FeatureMaps()
        {
            CreateMap<FeatureRequestModel, Feature>();
        }
    }
}
