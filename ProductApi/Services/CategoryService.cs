using AutoMapper;
using MongoDB.Driver;
using ProductApi.Models.Request;
using ProductApi.Models.Response;
using ProductApi.Services.Interfaces;

namespace ProductApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMongoDatabase database, IMapper mapper)
        {
            _categoryCollection = database.GetCollection<Category>("Category");
            _mapper = mapper;
            _productCollection = database.GetCollection<Product>("Product");
        }

        public async Task<Category> CreateCategory(CategoryRequest categoryRequest)
        {
            var category = _mapper.Map<Category>(categoryRequest);
            await _categoryCollection.InsertOneAsync(category);
            return category;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categories = await _categoryCollection.Find(_ => true).ToListAsync();
            return _mapper.Map<IEnumerable<Category>>(categories);
        }

        public async Task<Category> GetCategoryById(string categoryId)
        {
            var category = await _categoryCollection.Find(c => c.Id == categoryId).FirstOrDefaultAsync();
            return _mapper.Map<Category>(category);
        }

        public async Task<Category> UpdateCategory(string categoryId, CategoryRequest categoryRequest)
        {
            var existingCategory = await _categoryCollection.Find(c => c.Id == categoryId).FirstOrDefaultAsync();
            if (existingCategory == null)
            {
                throw new InvalidOperationException("Category not found.");
            }

            var updatedCategory = _mapper.Map(categoryRequest, existingCategory);
            await _categoryCollection.ReplaceOneAsync(c => c.Id == categoryId, updatedCategory);
            return _mapper.Map<Category>(updatedCategory);
        }

        public async Task<bool> DeleteCategory(string categoryId)
        {
            var productWithCategory = await _productCollection
                .Find(p => p.CategoryId == categoryId)
                .FirstOrDefaultAsync();

            if (productWithCategory is not null)
            {
                throw new ArgumentException($"Cannot delete category with ID {categoryId} because it is associated with a product (Product ID: {productWithCategory.Id}).");
            }

            var result = await _categoryCollection.DeleteOneAsync(c => c.Id == categoryId);

            return result.DeletedCount > 0;
        }
    }
}
