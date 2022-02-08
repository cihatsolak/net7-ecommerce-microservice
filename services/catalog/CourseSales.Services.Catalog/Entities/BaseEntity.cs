namespace CourseSales.Services.Catalog.Entities
{
    public  interface IEntity
    {
    }

    public abstract class BaseEntity : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
