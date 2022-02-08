namespace CourseSales.Services.Catalog.Models.Response
{
    public class CourseResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedDate { get; set; }

        public FeatureResponseModel FeatureResponseModel{ get; set; }

        public string CategoryId { get; set; }
        public CategoryResponseModel CategoryResponseModel { get; set; }
    }
}
