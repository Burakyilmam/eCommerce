
using AutoMapper;
using eCommerce.Application.DTOs.Products;
using eCommerce.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts(CancellationToken cancellationToken)
    {
        var products = await _productService.GetProductsWithDetailsAsync(cancellationToken);

        var result = _mapper.Map<List<ProductDto>>(products);

        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetProductById")]
    public async Task<IActionResult> GetProductById(int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetProductDetailsAsync(id, cancellationToken);

        if (product is null) return NotFound();

        return Ok(_mapper.Map<ProductDto>(product));
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetProductBySlug(string slug, CancellationToken cancellationToken)
    {
        var product = await _productService.GetProductBySlugAsync(slug, cancellationToken);

        if (product is null) return NotFound();

        var result = _mapper.Map<ProductDto>(product);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto, CancellationToken cancellationToken)
    {
        var product = await _productService.CreateProductAsync(dto, cancellationToken);

        var result = _mapper.Map<ProductDto>(product);

        return CreatedAtRoute("GetProductById", new { id = product.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto, CancellationToken cancellationToken)
    {
        var updated = await _productService.UpdateProductAsync(id, dto, cancellationToken);

        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _productService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
