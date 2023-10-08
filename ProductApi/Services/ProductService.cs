using MongoDB.Bson;
using MongoDB.Driver;
using ProductApi.Models.Request;
using ProductApi.Models.Response;
using ProductApi.Services.Interfaces;
using System.Runtime.Intrinsics.X86;
using System;
using MongoDB.Bson.Serialization;
using AutoMapper;

namespace ProductApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;


        public ProductService(IMongoDatabase database, ICategoryService categoryService, IMapper mapper)
        {
            _productCollection = database.GetCollection<Product>("Product");
            _categoryService = categoryService;
            _mapper = mapper;
        }
        public async Task<Product> CreateProduct(ProductRequest productRequest)
        {
            var category = await _categoryService.GetCategoryById(productRequest.CategoryId);
            if (category is null)
            {
                throw new ArgumentException("Category not found.");
            }

            var product = new Product(productRequest.Name, productRequest.Description, productRequest.Price, productRequest.CategoryId, productRequest.Color ?? string.Empty);

            await _productCollection.InsertOneAsync(product);
            return product;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$lookup",
                    new BsonDocument
                    {
                        { "from", "Category" },
                        { "localField", "CategoryId" },
                        { "foreignField", "_id" },
                        { "as", "Category" }
                    }),
                new BsonDocument("$unwind", "$Category")
            };

            var products = await _productCollection.Aggregate<Product>(pipeline).ToListAsync();
            return products;
        }


        public async Task<Product> GetProductById(string id)
        {
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$match", new BsonDocument("_id", new ObjectId(id))),
                new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "Category" },
                    { "localField", "CategoryId" },
                    { "foreignField", "_id" },
                    { "as", "Category" }
                }),
                new BsonDocument("$unwind", "$Category")
            };

            var product = await _productCollection.Aggregate<Product>(pipeline).FirstOrDefaultAsync();

            return product;
        }
        public async Task<Product> UpdateProduct(string productId, ProductRequest productRequest)
        {
            var category = await _categoryService.GetCategoryById(productRequest.CategoryId);
            if (category is null)
            {
                throw new ArgumentException("Category not found.");
            }

            var filter = Builders<Product>.Filter.Eq(p => p.Id, productId);
            var update = Builders<Product>.Update
                .Set(p => p.Name, productRequest.Name)
                .Set(p => p.Description, productRequest.Description)
                .Set(p => p.Price, productRequest.Price)
                .Set(p => p.CategoryId, productRequest.CategoryId)
                .Set(p => p.Color, productRequest.Color ?? string.Empty);

            var options = new FindOneAndUpdateOptions<Product>
            {
                ReturnDocument = ReturnDocument.After
            };

            var updatedProduct = await _productCollection.FindOneAndUpdateAsync(filter, update, options);
            return updatedProduct;
        }

        public async Task<bool> DeleteProduct(string productId)
        {
            var result = await _productCollection.DeleteOneAsync(p => p.Id == productId);
            return result.DeletedCount > 0;
        }
    }
}
