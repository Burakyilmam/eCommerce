using eCommerce.Application.Abstractions;
using eCommerce.Domain.Entities;
using eCommerce.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Infrastructure.Repositories;

public class BrandRepository : GenericRepository<Brand>, IBrandRepository
{
    public BrandRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Brand?> GetBrandBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug && x.IsActive, cancellationToken);
    }

    public async Task<IReadOnlyList<Brand>> GetActiveBrandsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.Name).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Brand>> GetBrandsWithProductsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .Where(x => x.IsActive)
            .Include(x => x.Products.Where(p => p.IsActive))
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }
}