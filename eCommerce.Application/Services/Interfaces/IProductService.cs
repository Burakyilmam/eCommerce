using eCommerce.Domain.Entities;

namespace eCommerce.Application.Services.Interfaces;

public interface IProductService : IGenericService<Product>
{
    Task<Product?> GetProductBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Product>> GetNewestAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Product>> GetMostViewedAsync(CancellationToken cancellationToken = default);
}