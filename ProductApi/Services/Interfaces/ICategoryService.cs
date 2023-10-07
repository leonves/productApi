using ProductApi.Models.Request;
using ProductApi.Models.Response;

namespace ProductApi.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> CreateCategory(CategoryRequest categoryRequest);
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategoryById(string categoryId);
        Task<Category> UpdateCategory(string categoryId, CategoryRequest categoryRequest);
        Task<bool> DeleteCategory(string categoryId);
    }
}
