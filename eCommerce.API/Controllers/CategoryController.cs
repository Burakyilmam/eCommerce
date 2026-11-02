using eCommerce.Application.Services.Interfaces;
using eCommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllAsync(cancellationToken);
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetByIdAsync(id, cancellationToken);

            if (category is null) return NotFound();

            return Ok(category);
        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetCategoryBySlug(string slug, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetCategoryBySlugAsync(slug, cancellationToken);

            if (category is null) return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category, CancellationToken cancellationToken)
        {
            await _categoryService.AddAsync(category, cancellationToken);

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category, CancellationToken cancellationToken)
        {
            if (id != category.Id) return BadRequest("URL üzerindeki ID ile kategori ID eşleşmiyor.");

            var existingCategory = await _categoryService.GetByIdAsync(id, cancellationToken);

            if (existingCategory is null) return NotFound();

            await _categoryService.UpdateAsync(category, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _categoryService.DeleteAsync(id, cancellationToken);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCategories(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetActiveCategoriesAsync(cancellationToken);
            return Ok(categories);
        }

        [HttpGet("root")]
        public async Task<IActionResult> GetRootCategories(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetRootCategoriesAsync(cancellationToken);
            return Ok(categories);
        }

        [HttpGet("{id:int}/subcategories")]
        public async Task<IActionResult> GetSubCategories(int id, CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetSubCategoriesAsync(id, cancellationToken);
            return Ok(categories);
        }
    }
}
