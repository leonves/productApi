using ProductApi.Models.Request;
using ProductApi.Models.Response;

namespace ProductApi.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProduct(ProductRequest productRequest);
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProductById(string productId);
        Task<Product> UpdateProduct(string productId, ProductRequest productRequest);
        Task<bool> DeleteProduct(string productId);
    }
}
