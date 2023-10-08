using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Moq;
using ProductApi.Models.Request;
using ProductApi.Models.Response;
using ProductApi.Services;
using ProductApi.Services.Interfaces;

namespace CategoryServiceTest
{

    public class CategoryServiceTests
    {
        private ICategoryService _categoryService;
        private IMongoCollection<Category> _categoryCollection;

        public CategoryServiceTests()
        {
            var mockCollection = new Mock<IMongoCollection<Category>>();

            var mockCategoryService = new Mock<ICategoryService>();
          
            mockCategoryService.Setup(x => x.CreateCategory(It.IsAny<CategoryRequest>()))
                  .ReturnsAsync(new Category("Name", "Description"));

            mockCategoryService.Setup(x => x.GetCategoryById(It.IsAny<string>())).ReturnsAsync((string categoryId) =>
            {
                if (categoryId == "1")
                {
                    return new Category("1", "CategoryName", "CategoryDescription");
                }
                else
                {
                    return null;
                }
            });

            mockCategoryService.Setup(x => x.UpdateCategory(It.IsAny<string>(), It.IsAny<CategoryRequest>()))
            .ReturnsAsync((string categoryId, CategoryRequest categoryRequest) =>
            {
                if (categoryId == "1")
                {
                    var updatedCategory = new Category("1", categoryRequest.Name, categoryRequest.Description);

                    return updatedCategory;
                }
                else
                {
                    return null;
                }
            });

            mockCategoryService.Setup(x => x.DeleteCategory(It.IsAny<string>()))
            .ReturnsAsync((string categoryId) =>
            {
                if (categoryId == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });

            var Categories = new List<Category>
            {
                new Category("1", "CategoryName 1", "CategoryDescription 1"),
                new Category("2", "CategoryName 2", "CategoryDescription 2"),
                new Category("3", "CategoryName 3", "CategoryDescription 3"),
                new Category("4", "CategoryName 4", "CategoryDescription 4"),
                new Category("5", "CategoryName 5", "CategoryDescription 5")
            };

            mockCategoryService.Setup(x => x.GetCategories())
            .ReturnsAsync(() =>
            {
                return Categories;
            });

            var serviceProvider = new ServiceCollection()
                .AddScoped(_ => mockCollection.Object)
                .AddScoped(_ => mockCategoryService.Object)
                .AddScoped<CategoryService>()
                .BuildServiceProvider();

            _categoryService = serviceProvider.GetRequiredService<ICategoryService>();
            _categoryCollection = serviceProvider.GetRequiredService<IMongoCollection<Category>>();
        }

        [Fact]
        public async Task CreateCategory_Should_CreateCategory_And_ReturnCategoryResponse()
        {
            // Arrange
            var categoryRequest = new CategoryRequest { Name = "Name", Description = "Description" };

            // Act
            var categoryResponse = await _categoryService.CreateCategory(categoryRequest);

            // Assert
            Assert.NotNull(categoryResponse);
            Assert.Equal(categoryRequest.Name, categoryResponse.Name);
            Assert.Equal(categoryRequest.Description, categoryResponse.Description);
        }

        [Fact]
        public async Task GetCategories_Should_ReturnListOfCategoryResponse()
        {
            // Act
            var categoryResponses = await _categoryService.GetCategories();

            // Assert
            Assert.NotNull(categoryResponses);
            Assert.IsType<List<Category>>(categoryResponses);
        }

        [Fact]
        public async Task GetCategoryById_Should_ReturnCategoryResponse_When_CategoryExists()
        {
            // Arrange
            var categoryId = "1";

            // Act
            var categoryResponse = await _categoryService.GetCategoryById(categoryId);

            // Assert
            Assert.NotNull(categoryResponse);
            Assert.IsType<Category>(categoryResponse);
        }

        [Fact]
        public async Task GetCategoryById_Should_ReturnNull_When_CategoryDoesNotExist()
        {
            // Arrange
            var categoryId = "2";

            // Act
            var categoryResponse = await _categoryService.GetCategoryById(categoryId);

            // Assert
            Assert.Null(categoryResponse);
        }

        [Fact]
        public async Task UpdateCategory_Should_UpdateCategory_And_ReturnUpdatedCategoryResponse()
        {
            // Arrange
            var categoryId = "1";
            var categoryRequest = new CategoryRequest { Name = "Updated Category", Description = "Updated Description" };

            // Act
            var categoryResponse = await _categoryService.UpdateCategory(categoryId, categoryRequest);

            // Assert
            Assert.NotNull(categoryResponse);
            Assert.Equal(categoryRequest.Name, categoryResponse.Name);
            Assert.Equal(categoryRequest.Description, categoryResponse.Description);
        }

        [Fact]
        public async Task UpdateCategory_Should_ReturnNull_When_CategoryDoesNotExist()
        {
            // Arrange
            var categoryId = "2";
            var categoryRequest = new CategoryRequest { Name = "Updated Category", Description = "Updated Description" };

            // Act
            var categoryResponse = await _categoryService.UpdateCategory(categoryId, categoryRequest);

            // Assert
            Assert.Null(categoryResponse);
        }

        [Fact]
        public async Task DeleteCategory_Should_ReturnTrue_When_CategoryIsDeleted()
        {
            // Arrange
            var categoryId = "1";

            // Act
            var result = await _categoryService.DeleteCategory(categoryId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteCategory_Should_ReturnFalse_When_CategoryDoesNotExist()
        {
            // Arrange
            var categoryId = "2";

            // Act
            var result = await _categoryService.DeleteCategory(categoryId);

            // Assert
            Assert.False(result);
        }
    }

}