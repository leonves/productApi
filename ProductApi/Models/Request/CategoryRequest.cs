using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models.Request
{
    public class CategoryRequest
    {
        [Required(ErrorMessage = "The 'Name' field is required.")]
        [StringLength(100, ErrorMessage = "The 'Name' field must not exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The 'Description' field is required.")]
        [StringLength(150, ErrorMessage = "The 'Description' field must not exceed 150 characters.")]
        public string Description { get; set; }
    }
}
