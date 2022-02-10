namespace CourseSales.Services.Catalog.Data
{
    internal interface IMongoContext
    {
        IMongoCollection<Course> Courses { get; }
        IMongoCollection<Category> Categories { get; }

    }

    internal class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase mongoDatabase;

        public MongoContext(IDatabaseSetting databaseSetting)
        {
            MongoClient mongoClient = new(databaseSetting.ConnectionString);
            mongoDatabase = mongoClient.GetDatabase(databaseSetting.DatabaseName);

            Courses = mongoDatabase.GetCollection<Course>(databaseSetting.CourseCollectionName);
            Categories = mongoDatabase.GetCollection<Category>(databaseSetting.CategoryCollectionName);

        }

        public IMongoCollection<Course> Courses { get; }
        public IMongoCollection<Category> Categories { get; }
    }
}
