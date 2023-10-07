using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductApi.Models;
using ProductApi.Models.Request;
using ProductApi.Models.Response;
using ProductApi.Services.Interfaces;
using System;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly ICategoryService _categoryService;
        public CategoryController(IMongoDatabase database, IMapper mapper, ICategoryService categoryService)
        {
            _categoryCollection = database.GetCollection<Category>("Category");
            _mapper = mapper;
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryRequest category)
        {
            try
            {
                var categoryResponse = await _categoryService.CreateCategory(category);
                return Ok(categoryResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);

                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(string id, CategoryRequest category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedCategory = await _categoryService.UpdateCategory(id, category);

                if (updatedCategory == null)
                {
                    return NotFound("Category not found");
                }

                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            try
            {
                var deletedCategory = await _categoryService.DeleteCategory(id);

                if (deletedCategory == false)
                {
                    return NotFound("Category not found");
                }

                return Ok(deletedCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

    }
}
