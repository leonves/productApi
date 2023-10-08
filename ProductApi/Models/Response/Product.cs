using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProductApi.Models.Response
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; private set; }

        [BsonRequired]
        public string Name { get; private set; }

        [BsonRequired]
        public string Description { get; private set; }

        [BsonRequired]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; private set; }

        [BsonRequired]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; private set; }

        public string Color { get; private set; }
        
        public Category Category { get; private set; }


        public Product(string name, string description, decimal price, string categoryId, string color)
        {
            ValidateInput(name, description, price, categoryId, color);

            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
            Color = color;
        }

        private static void ValidateInput(string name, string description, decimal price, string categoryId, string color)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be null or empty.", nameof(description));
            }

            if (!description.StartsWith(name + " - "))
            {
                throw new ArgumentException("The 'Description' field must contain the product name followed by a hyphen and the description.", nameof(description));
            }

            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(categoryId))
            {
                throw new ArgumentNullException(nameof(categoryId), "Category cannot be null.");
            }

        }

    }
}
