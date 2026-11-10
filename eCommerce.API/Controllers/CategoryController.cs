
using AutoMapper;
using eCommerce.Application.DTOs.Categories;
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
        private readonly IMapper _mapper;

        public CategoryController(
            ICategoryService categoryService,
            IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetAllCategories(
            CancellationToken cancellationToken)
        {
            var categories = await _categoryService
                .GetAllAsync(cancellationToken);

            var result = _mapper.Map<List<CategoryDto>>(categories);

            return Ok(result);
        }

        // GET: api/Category/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById(
            int id,
            CancellationToken cancellationToken)
        {
            var category = await _categoryService
                .GetByIdAsync(id, cancellationToken);

            if (category is null)
                return NotFound();

            return Ok(_mapper.Map<CategoryDto>(category));
        }

        // GET: api/Category/slug/elektronik
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetCategoryBySlug(
            string slug,
            CancellationToken cancellationToken)
        {
            var category = await _categoryService
                .GetCategoryBySlugAsync(slug, cancellationToken);

            if (category is null)
                return NotFound();

            return Ok(_mapper.Map<CategoryDto>(category));
        }

        // POST: api/Category
        [HttpPost]
        public async Task<IActionResult> CreateCategory(
            [FromBody] CreateCategoryDto dto,
            CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(dto);

            await _categoryService.AddAsync(
                category, cancellationToken);

            var result = _mapper.Map<CategoryDto>(category);

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = category.Id },
                result
            );
        }

        // PUT: api/Category/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            [FromBody] UpdateCategoryDto dto,
            CancellationToken cancellationToken)
        {
            var category = await _categoryService
                .GetByIdAsync(id, cancellationToken);

            if (category is null)
                return NotFound();

            // Mevcut entity üzerine DTO alanlarını aktar.
            _mapper.Map(dto, category);

            await _categoryService.UpdateAsync(
                category, cancellationToken);

            return NoContent();
        }

        // DELETE: api/Category/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(
            int id,
            CancellationToken cancellationToken)
        {
            try
            {
                await _categoryService.DeleteAsync(
                    id, cancellationToken);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Category/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCategories(
            CancellationToken cancellationToken)
        {
            var categories = await _categoryService
                .GetActiveCategoriesAsync(cancellationToken);

            var result = _mapper.Map<List<CategoryDto>>(categories);

            return Ok(result);
        }

        // GET: api/Category/root
        [HttpGet("root")]
        public async Task<IActionResult> GetRootCategories(
            CancellationToken cancellationToken)
        {
            var categories = await _categoryService
                .GetRootCategoriesAsync(cancellationToken);

            var result = _mapper.Map<List<CategoryDto>>(categories);

            return Ok(result);
        }

        // GET: api/Category/5/subcategories
        [HttpGet("{id:int}/subcategories")]
        public async Task<IActionResult> GetSubCategories(
            int id,
            CancellationToken cancellationToken)
        {
            var categories = await _categoryService
                .GetSubCategoriesAsync(id, cancellationToken);

            var result = _mapper.Map<List<CategoryDto>>(categories);

            return Ok(result);
        }
    }
}
