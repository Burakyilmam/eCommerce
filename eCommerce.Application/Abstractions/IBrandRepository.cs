using eCommerce.Domain.Entities;

namespace eCommerce.Application.Abstractions;

public interface IBrandRepository : IGenericRepository<Brand>
{
    Task<Brand?> GetBrandBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Brand>> GetActiveBrandsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Brand>> GetBrandsWithProductsAsync(CancellationToken cancellationToken = default);
}