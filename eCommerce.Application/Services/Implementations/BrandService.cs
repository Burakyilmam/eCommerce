using AutoMapper;
using eCommerce.Application.Abstractions;
using eCommerce.Application.DTOs.Brand;
using eCommerce.Application.Services.Interfaces;
using eCommerce.Domain.Entities;

namespace eCommerce.Application.Services.Implementations;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BrandService(IBrandRepository brandRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _brandRepository = brandRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<BrandDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var brands = await _brandRepository.GetAllAsync(cancellationToken);

        return _mapper.Map<IReadOnlyList<BrandDto>>(brands);
    }

    public async Task<BrandDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var brand = await _brandRepository.GetByIdAsync(id, cancellationToken);

        return brand is null ? null : _mapper.Map<BrandDto>(brand);
    }

    public async Task<BrandDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var brand = await _brandRepository.GetBrandBySlugAsync(slug, cancellationToken);

        return brand is null ? null : _mapper.Map<BrandDto>(brand);
    }

    public async Task<IReadOnlyList<BrandDto>> GetActiveBrandsAsync(CancellationToken cancellationToken = default)
    {
        var brands = await _brandRepository.GetActiveBrandsAsync(cancellationToken);

        return _mapper.Map<IReadOnlyList<BrandDto>>(brands);
    }

    public async Task<BrandDto> AddAsync(CreateBrandDto dto, CancellationToken cancellationToken = default)
    {
        var brand = _mapper.Map<Brand>(dto);

        await _brandRepository.AddAsync(brand, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BrandDto>(brand);
    }

    public async Task<BrandDto?> UpdateAsync(int id, UpdateBrandDto dto, CancellationToken cancellationToken = default)
    {
        var brand = await _brandRepository.GetByIdAsync(id, cancellationToken);

        if (brand is null) return null;

        _mapper.Map(dto, brand);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BrandDto>(brand);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var brand = await _brandRepository.GetByIdAsync(id, cancellationToken);

        if (brand is null)
            return false;

        _brandRepository.Delete(brand);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}