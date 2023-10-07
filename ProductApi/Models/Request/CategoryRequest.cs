using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProductApi.Models.Request
{
    public class CategoryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
