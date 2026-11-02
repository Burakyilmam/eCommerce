using eCommerce.Domain.Entities;

namespace eCommerce.Application.Abstractions;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category?> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Category>> GetRootCategoriesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Category>> GetSubCategoriesAsync(int parentCategoryId, CancellationToken cancellationToken = default);

    Task<Category?> GetCategoryWithDetailsAsync(int categoryId, CancellationToken cancellationToken = default);
}