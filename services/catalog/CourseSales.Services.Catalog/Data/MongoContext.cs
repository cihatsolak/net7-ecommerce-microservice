namespace CourseSales.Services.Catalog.Data
{
    public interface IMongoContext
    {
        IMongoCollection<Course> Courses { get; }
        IMongoCollection<Category> Categories { get; }

    }

    public class MongoContext : IMongoContext
    {
        public MongoContext(IDatabaseSetting databaseSetting)
        {
            MongoClient mongoClient = new(databaseSetting.ConnectionString);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(databaseSetting.DatabaseName);

            Courses = mongoDatabase.GetCollection<Course>(databaseSetting.CourseCollectionName);
            Categories = mongoDatabase.GetCollection<Category>(databaseSetting.CategoryCollectionName);
        }

        public IMongoCollection<Course> Courses { get; }
        public IMongoCollection<Category> Categories { get; }
    }
}
