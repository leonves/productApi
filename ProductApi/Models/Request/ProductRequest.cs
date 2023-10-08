using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models.Request
{
    public class ProductRequest
    {
        [Required(ErrorMessage = "The 'Name' field is required.")]
        [StringLength(100, ErrorMessage = "The 'Name' field must have at most {1} characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The 'Description' field is required.")]
        [StringLength(150, ErrorMessage = "The 'Description' field must have at most {1} characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The 'Price' field is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "The 'Price' field must be a non-negative numeric value.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The 'CategoryId' field is required.")]
        public string CategoryId { get; set; }

        public string? Color { get; set; }
    }
}
