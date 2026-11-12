using eCommerce.Application.DTOs.Brand;
using eCommerce.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBrands(CancellationToken cancellationToken)
    {
        var brands = await _brandService.GetAllAsync(cancellationToken);
        return Ok(brands);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBrandById(int id, CancellationToken cancellationToken)
    {
        var brand = await _brandService.GetByIdAsync(id, cancellationToken);

        if (brand is null) return NotFound();

        return Ok(brand);
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBrandBySlug(string slug, CancellationToken cancellationToken)
    {
        var brand = await _brandService.GetBySlugAsync(slug, cancellationToken);

        if (brand is null) return NotFound();

        return Ok(brand);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveBrands(CancellationToken cancellationToken)
    {
        var brands = await _brandService.GetActiveBrandsAsync(cancellationToken);
        return Ok(brands);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateBrand([FromBody] CreateBrandDto dto, CancellationToken cancellationToken)
    {
        var brand = await _brandService.AddAsync(dto, cancellationToken);

        return CreatedAtAction(nameof(GetBrandById), new { id = brand.Id }, brand);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBrand(int id, [FromBody] UpdateBrandDto dto, CancellationToken cancellationToken)
    {
        var brand = await _brandService.UpdateAsync(id, dto, cancellationToken);

        if (brand is null) return NotFound();

        return Ok(brand);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBrand(int id, CancellationToken cancellationToken)
    {
        var deleted = await _brandService.DeleteAsync(id, cancellationToken);

        if (!deleted) return NotFound();

        return NoContent();
    }
}