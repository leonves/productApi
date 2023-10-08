using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductApi.Models.Response
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; private set; }
        [BsonRequired]
        public string Name { get; private set; }

        [BsonRequired]
        public string Description { get; private set; }

        public Category(string name, string description)
        {
            ValidationInput(name, description);
            Name = name;
            Description = description;
        }

        public Category(string id, string name, string description)
        {
            ValidationInput(name, description);
            Id = id;
            Name = name;
            Description = description;
        }

        private static void ValidationInput(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be null or empty.", nameof(description));
            }
        }
    }
}
