namespace CourseSales.Services.Catalog.Models.Request
{
    public class UpdateCourseRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public string UserId { get; set; }
        public string CategoryId { get; set; }
        public FeatureRequestModel FeatureRequestModel { get; set; }
    }
}
