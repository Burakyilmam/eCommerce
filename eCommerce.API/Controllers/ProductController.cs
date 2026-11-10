
using AutoMapper;
using eCommerce.Application.DTOs.Products;
using eCommerce.Application.Services.Interfaces;
using eCommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductController(
        IProductService productService,
        IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    // GET: api/Product
    [HttpGet]
    public async Task<IActionResult> GetAllProducts(CancellationToken cancellationToken)
    {
        var products = await _productService.GetProductsWithDetailsAsync(cancellationToken);

        var result = _mapper.Map<List<ProductDto>>(products);

        return Ok(result);
    }

    // GET: api/Product/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetProductDetailsAsync(id, cancellationToken);

        if (product is null) return NotFound();

        var result = _mapper.Map<ProductDto>(product);

        return Ok(result);
    }

    // GET: api/Product/slug/example
    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetProductBySlug(string slug, CancellationToken cancellationToken)
    {
        var product = await _productService.GetProductBySlugAsync(slug, cancellationToken);

        if (product is null) return NotFound();

        var result = _mapper.Map<ProductDto>(product);

        return Ok(result);
    }

    // POST: api/Product
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(dto);

        await _productService.AddAsync(product, cancellationToken);

        var result = _mapper.Map<ProductDto>(product);

        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, result);
    }

    // PUT: api/Product/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);

        if (product is null) return NotFound();

        _mapper.Map(dto, product);

        await _productService.UpdateAsync(product, cancellationToken);

        return NoContent();
    }

    // DELETE: api/Product/5
    [HttpDelete("{id:int}")]
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
