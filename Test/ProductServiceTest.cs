using AutoMapper;
using MongoDB.Driver;
using Moq;
using ProductApi.Models.Response;
using ProductApi.Services.Interfaces;
using ProductApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using ProductApi.Models.Request;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Linq.Expressions;
using MongoDB.Driver.Linq;

namespace Test
{
    public class ProductServiceTest
    {
        private IProductService _productService;
        private Mock<IProductService> _mockProductService;
        private Mock<IMongoCollection<Product>> _mockProductCollection;


        public ProductServiceTest()
        {
            _mockProductService = new Mock<IProductService>();
            _mockProductCollection = new Mock<IMongoCollection<Product>>();

            _mockProductService.Setup(x => x.DeleteProduct(It.IsAny<string>()))
            .ReturnsAsync((string productId) =>
            {
                if (productId == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });

            _mockProductService.Setup(x => x.UpdateProduct(It.IsAny<string>(), It.IsAny<ProductRequest>()))
            .ReturnsAsync((string productId, ProductRequest productRequest) =>
            {
                if (productId == "1")
                {
                    var updatedProduct = new Product(productId, productRequest.Name, productRequest.Description, productRequest.Price, productRequest.CategoryId, productRequest.Color);

                    return updatedProduct;
                }
                else
                {
                    return null;
                }
            });

            var serviceProvider = new ServiceCollection()
                .AddScoped(_ => _mockProductCollection.Object)
                .AddScoped(_ => _mockProductService.Object)
                .BuildServiceProvider();

            _productService = serviceProvider.GetRequiredService<IProductService>();
        }

        [Fact]
        public async Task CreateProduct_Should_CreateProduct_And_ReturnProduct()
        {
            // Arrange
            var productRequest = new ProductRequest
            {
                Name = "Test Product",
                Description = "Test Product - Description",
                Price = 100.0m,
                CategoryId = "categoryId",
                Color = "Red"
            };


            _mockProductService.Setup(ps => ps.CreateProduct(It.IsAny<ProductRequest>()))
            .ReturnsAsync((ProductRequest request) =>
            {
                if (productRequest.CategoryId == "nonExistentCategoryId")
                {
                    throw new ArgumentException("Category not found.");
                }
                return new Product("1", productRequest.Name, productRequest.Description, productRequest.Price, productRequest.CategoryId, productRequest.Color);
            });

            // Act
            var result = await _productService.CreateProduct(productRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal(productRequest.Name, result.Name);
            Assert.Equal(productRequest.Description, result.Description);
            Assert.Equal(productRequest.Price, result.Price);
            Assert.Equal(productRequest.CategoryId, result.CategoryId);
            Assert.Equal(productRequest.Color, result.Color);
        }

        [Fact]
        public async Task CreateProduct_Should_ThrowException_When_CategoryNotFound()
        {
            // Arrange
            var productRequest = new ProductRequest
            {
                Name = "Test Product",
                Description = "Test Product - Description",
                Price = 100.0m,
                CategoryId = "nonExistentCategoryId",
                Color = "Red"
            };

            _mockProductService.Setup(ps => ps.CreateProduct(It.IsAny<ProductRequest>()))
            .ReturnsAsync((ProductRequest request) =>
            {
                if (productRequest.CategoryId == "nonExistentCategoryId")
                {
                    throw new ArgumentException("Category not found.");
                }
                return new Product("1", productRequest.Name, productRequest.Description, productRequest.Price, productRequest.CategoryId, productRequest.Color);
            });

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _productService.CreateProduct(productRequest));
        }


        [Fact]
        public async Task GetProducts_Should_ReturnListOfProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product("1", "Product1", "Product1 - Description1", 10.0m, "categoryId1", "Color1"),
                new Product("2", "Product2", "Product2 - Description2", 20.0m, "categoryId2", "Color2"),
            };

            _mockProductService.Setup(x => x.GetProducts())
            .ReturnsAsync(() =>
            {
                return products;
            });

            // Act
            var result = await _productService.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Product>>(result);
            Assert.Equal(products.Count, result.Count());
        }

        [Fact]
        public async Task DeleteProduct_Should_ReturnTrue_When_ProductIsDeleted()
        {
            // Arrange
            var productId = "1";


            // Act
            var result = await _productService.DeleteProduct(productId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProduct_Should_ReturnFalse_When_ProductDoesNotExist()
        {
            // Arrange
            var productId = "0";

            // Act
            var result = await _productService.DeleteProduct(productId);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public async Task UpdateProduct_Should_UpdateProduct_And_ReturnUpdatedProduct()
        {
            // Arrange
            var productId = "1";
            var productRequest = new ProductRequest
            {
                Name = "Updated Product",
                Description = "Updated Product - Updated Description",
                Price = 50.0m,
                CategoryId = "categoryId2",
                Color = "UpdatedColor"
            };

            // Act
            var result = await _productService.UpdateProduct(productId, productRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal(productRequest.Name, result.Name);
            Assert.Equal(productRequest.Description, result.Description);
            Assert.Equal(productRequest.Price, result.Price);
            Assert.Equal(productRequest.CategoryId, result.CategoryId);
            Assert.Equal(productRequest.Color, result.Color);
        }


        [Fact]
        public async Task UpdateCategory_Should_ReturnNull_When_CategoryDoesNotExist()
        {
            // Arrange
            var productId = "2";
            var productRequest = new ProductRequest { Name = "Updated Product", Description = "Updated Description" };

            // Act
            var productResponse = await _productService.UpdateProduct(productId, productRequest);

            // Assert
            Assert.Null(productResponse);
        }
    }
}
