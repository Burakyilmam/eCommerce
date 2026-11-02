using eCommerce.Domain.Entities;

namespace eCommerce.Application.Services.Interfaces;

public interface ICategoryService : IGenericService<Category>
{
    Task<Category?> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Category>> GetRootCategoriesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Category>> GetSubCategoriesAsync(int parentCategoryId, CancellationToken cancellationToken = default);
}