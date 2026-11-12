using eCommerce.Application.Abstractions;
using eCommerce.Domain.Entities;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<Product?> GetProductBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetProductsWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetNewestProductsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetProductsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetProductsByBrandAsync(int brandId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetProductsByHighestPriceAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetProductsByLowestPriceAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetMostViewedProductsAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetProductDetailsAsync(int productId, CancellationToken cancellationToken = default);
    Task<bool> SlugExistsAsync(string slug, int? excludeProductId = null, CancellationToken cancellationToken = default);
}