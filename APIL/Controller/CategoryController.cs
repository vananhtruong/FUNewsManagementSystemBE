using BusinessObject.Entities;
using FUNews.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAppAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _cS;
        public CategoryController(ICategoryService cS)
        {
            _cS = cS;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _cS.GetAllCategoriesAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(short id)
        {
            var category = await _cS.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            if (category == null)
                return BadRequest("Category cannot be null.");
            await _cS.AddCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.CategoryId }, category);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] Category category)
        {
            if (category == null)
                return BadRequest("Category cannot be null.");
            await _cS.UpdateCategoryAsync(category);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(short id)
        {
            try
            {
                await _cS.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCategories()
        {
            var activeCategories = await _cS.GetActiveCategoriesAsync();
            return Ok(activeCategories);
        }
    }
}
