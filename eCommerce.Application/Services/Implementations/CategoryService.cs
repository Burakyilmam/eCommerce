using eCommerce.Application.Abstractions;
using eCommerce.Application.Services.Interfaces;
using eCommerce.Domain.Entities;

namespace eCommerce.Application.Services.Implementations;

public class CategoryService : GenericService<Category>, ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : base(categoryRepository, unitOfWork)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Category?> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _categoryRepository.GetCategoryBySlugAsync(slug, cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _categoryRepository.GetActiveCategoriesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> GetRootCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _categoryRepository.GetRootCategoriesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> GetSubCategoriesAsync(int parentCategoryId, CancellationToken cancellationToken = default)
    {
        return await _categoryRepository.GetSubCategoriesAsync(parentCategoryId, cancellationToken);
    }
}