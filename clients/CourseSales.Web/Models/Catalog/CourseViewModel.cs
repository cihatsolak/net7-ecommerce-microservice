namespace CourseSales.Web.Models.Catalog
{
    public class CourseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription
        {
            get
            {
                if (Description.Length > 100)
                    return string.Concat(Description, "...");

                return Description;
            }
        }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string ImagePath { get; set; }
        public string StockImagePath { get; set; }
        public DateTime CreatedDate { get; set; }

        public FeatureViewModel Feature { get; set; }

        public string CategoryId { get; set; }
        public CategoryViewModel Category { get; set; }
    }
}
