namespace CourseSales.Services.Catalog.Settings
{
    public interface IDatabaseSetting
    {
        string CourseCollectionName { get; init; }
        string CategoryCollectionName { get; init; }
        string ConnectionString { get; init; }
        string DatabaseName { get; init; }
    }

    public record DatabaseSetting : IDatabaseSetting
    {
        public string CourseCollectionName { get; init; }
        public string CategoryCollectionName { get; init; }
        public string ConnectionString { get; init; }
        public string DatabaseName { get; init; }
    }
}
