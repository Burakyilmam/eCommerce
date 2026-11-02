using eCommerce.Application.Abstractions;
using eCommerce.Domain.Entities;
using eCommerce.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Infrastructure.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Category?> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug && x.IsActive, cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.DisplayOrder).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> GetRootCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().
            Where(x => x.IsActive && x.ParentCategoryId == null).
            OrderBy(x => x.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> GetSubCategoriesAsync(int parentCategoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .Where(x => x.IsActive && x.ParentCategoryId == parentCategoryId)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetCategoryWithDetailsAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .Include(x => x.SubCategories)
            .Include(x => x.CategoryAttributes)
                .ThenInclude(x => x.ProductAttribute)
            .FirstOrDefaultAsync(x => x.Id == categoryId && x.IsActive, cancellationToken);
    }
}