using Ecommerce_Library.Models;
using Ecommerce_Library.Responses;
using Ecommerce_Server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategory categoryService) : ControllerBase
    {
        [HttpGet("All-Categories")]
        public async Task<ActionResult<List<Product>>> GetAllCategories()
        {
            var products = await categoryService.GetAllCategories();
            return Ok(products);
        }
        [HttpPost("Add-Category")]
        public async Task<ActionResult<ServiceResponse>> AddCategory(Category model)
        {
            if (model is null)
                return BadRequest("Model is null");

            var response = await categoryService.AddCategory(model);

            return Ok(response);
        }
    }
}
