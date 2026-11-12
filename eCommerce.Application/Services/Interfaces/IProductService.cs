using eCommerce.Application.DTOs.Products;
using eCommerce.Domain.Entities;

namespace eCommerce.Application.Services.Interfaces;

public interface IProductService : IGenericService<Product>
{
    Task<Product?> GetProductBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Product>> GetNewestAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Product>> GetMostViewedAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Product>> GetProductsWithDetailsAsync(CancellationToken cancellationToken = default);

    Task<Product?> GetProductDetailsAsync(int productId, CancellationToken cancellationToken = default);

    Task<Product> CreateProductAsync(CreateProductDto dto, CancellationToken cancellationToken = default);

    Task<bool> UpdateProductAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken = default);
}