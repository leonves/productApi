using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductApi.Models;
using ProductApi.Models.Request;
using ProductApi.Models.Response;
using System;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Category> _categoryCollection;
        public CategoryController(IMongoDatabase database, IMapper mapper)
        {
            _categoryCollection = database.GetCollection<Category>("Category");
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryRequest category)
        {
            try
            {
                var categoryResponse = _mapper.Map<Category>(category);
                await _categoryCollection.InsertOneAsync(categoryResponse);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
