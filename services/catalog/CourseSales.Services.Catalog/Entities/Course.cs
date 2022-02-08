namespace CourseSales.Services.Catalog.Entities
{
    public class Course : BaseEntity
    {
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        [BsonRepresentation(BsonType.DateTime)] //Note: Parametre tipi
        public DateTime CreatedDate { get; set; }

        public string UserId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }


        #region Relation ships
        public Feature Feature { get; set; } //Note: Feature ile Course arasında ilişki

        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }

        [BsonIgnore] //Note: MongoDB'ye bu property yansıtma.
        public Category Category { get; set; }
        #endregion
    }
}
