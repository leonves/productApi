using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models.Request;
using ProductApi.Services;
using ProductApi.Services.Interfaces;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequest productRequest)
        {
            try
            {
                var productResponse = await _productService.CreateProduct(productRequest);
                return Ok(productResponse);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductsById(string id)
        {
            try
            {
                var product = await _productService.GetProductById(id);

                if (product == null)
                {
                    return NotFound("Product not found");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, ProductRequest product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedCategory = await _productService.UpdateProduct(id, product);

                if (updatedCategory == null)
                {
                    return NotFound("Product not found");
                }

                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                var deletedProduct = await _productService.DeleteProduct(id);

                if (deletedProduct == false)
                {
                    return NotFound("Product not found");
                }

                return Ok(deletedProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

    }
}
