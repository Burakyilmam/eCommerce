using eCommerce.Application.DTOs.Brand;

namespace eCommerce.Application.Services.Interfaces;

public interface IBrandService
{
    Task<IReadOnlyList<BrandDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<BrandDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<BrandDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BrandDto>> GetActiveBrandsAsync(CancellationToken cancellationToken = default);

    Task<BrandDto> AddAsync(CreateBrandDto dto, CancellationToken cancellationToken = default);

    Task<BrandDto?> UpdateAsync(int id, UpdateBrandDto dto, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}