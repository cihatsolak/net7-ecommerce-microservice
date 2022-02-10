namespace CourseSales.Services.Catalog.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CourseMaps();
            CategoryMaps();
        }

        private void CourseMaps()
        {
            CreateMap<Course, CourseResponseModel>()
                .ForMember(dest => dest.FeatureResponseModel, opt => opt.MapFrom(src => src.Feature));

            CreateMap<Category, CategoryResponseModel>();
            CreateMap<Feature, FeatureResponseModel>();

            CreateMap<AddCourseRequstModel, Course>();
            CreateMap<UpdateCourseRequstModel, Course>();
        }

        private void CategoryMaps()
        {
            CreateMap<AddCategoryRequestModel, Category>();
        }
    }
}
