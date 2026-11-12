
using eCommerce.Domain.Entities;
using eCommerce.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {

        }

        private IQueryable<Product> ProductsWithDetails()
        {
            return _dbSet
                .AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Images)
                .Include(x => x.Variants)
                    .ThenInclude(v => v.SellerListings);
        }

        public async Task<IReadOnlyList<Product>> GetProductsWithDetailsAsync(CancellationToken cancellationToken = default)
        {
            return await ProductsWithDetails().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Product>> GetNewestProductsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Product>> GetProductsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.CategoryId == categoryId && x.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Product>> GetProductsByBrandAsync(int brandId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.BrandId == brandId && x.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Product>> GetProductsByHighestPriceAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.Variants
                    .SelectMany(v => v.SellerListings)
                    .Where(l => l.IsActive)
                    .Select(l => (decimal?)l.Price)
                    .Max())
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Product>> GetProductsByLowestPriceAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Variants
                    .SelectMany(v => v.SellerListings)
                    .Where(l => l.IsActive)
                    .Select(l => (decimal?)l.Price)
                    .Min())
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Product>> GetMostViewedProductsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.ViewCount)
                .Take(10)
                .ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetProductDetailsAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await ProductsWithDetails().FirstOrDefaultAsync(x => x.Id == productId && x.IsActive, cancellationToken);
        }

        public async Task<Product?> GetProductBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await ProductsWithDetails().FirstOrDefaultAsync(x => x.Slug == slug && x.IsActive, cancellationToken);
        }

        public async Task<bool> SlugExistsAsync(string slug, int? excludeProductId = null, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(p => p.Slug == slug && (!excludeProductId.HasValue || p.Id != excludeProductId.Value), cancellationToken);
        }
    }
}
