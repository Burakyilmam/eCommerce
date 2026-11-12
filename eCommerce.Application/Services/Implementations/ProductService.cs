using AutoMapper;
using eCommerce.Application.Abstractions;
using eCommerce.Application.DTOs.Products;
using eCommerce.Application.Exceptions;
using eCommerce.Application.Services.Interfaces;
using eCommerce.Domain.Entities;

namespace eCommerce.Application.Services.Implementations;

public class ProductService : GenericService<Product>, IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IMapper _mapper;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : base(productRepository, unitOfWork)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _mapper = mapper;
    }

    public Task<Product?> GetProductBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return _productRepository.GetProductBySlugAsync(slug, cancellationToken);
    }

    public Task<IReadOnlyList<Product>> GetNewestAsync(CancellationToken cancellationToken = default)
    {
        return _productRepository.GetNewestProductsAsync(cancellationToken);
    }

    public Task<IReadOnlyList<Product>> GetMostViewedAsync(CancellationToken cancellationToken = default)
    {
        return _productRepository.GetMostViewedProductsAsync(cancellationToken);
    }

    public Task<IReadOnlyList<Product>> GetProductsWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return _productRepository.GetProductsWithDetailsAsync(cancellationToken);
    }

    public Task<Product?> GetProductDetailsAsync(int productId, CancellationToken cancellationToken = default)
    {
        return _productRepository.GetProductDetailsAsync(productId, cancellationToken);
    }

    public async Task<Product> CreateProductAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        await ValidateProductAsync(dto.CategoryId, dto.BrandId, dto.Slug, null, cancellationToken);

        var product = _mapper.Map<Product>(dto);

        await AddAsync(product, cancellationToken);

        return product;
    }

    public async Task<bool> UpdateProductAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var product = await GetByIdAsync(id, cancellationToken);

        if (product is null)
        {
            return false;
        }

        await ValidateProductAsync(dto.CategoryId, dto.BrandId, dto.Slug, id, cancellationToken);

        _mapper.Map(dto, product);

        await UpdateAsync(product, cancellationToken);

        return true;
    }

    private async Task ValidateProductAsync(
        int categoryId,
        int brandId,
        string slug,
        int? excludeProductId,
        CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);

        if (category is null)
        {
            throw new ArgumentException("The specified category was not found.");
        }

        if (!category.IsActive)
        {
            throw new ArgumentException("Products cannot be added to an inactive category.");
        }

        var brand = await _brandRepository.GetByIdAsync(brandId, cancellationToken);

        if (brand is null)
        {
            throw new ArgumentException("The specified brand was not found.");
        }

        if (!brand.IsActive)
        {
            throw new ArgumentException("Products cannot be added to an inactive brand.");
        }

        var slugExists = await _productRepository.SlugExistsAsync(slug, excludeProductId, cancellationToken);

        if (slugExists)
        {
            throw new ConflictException("This slug is already used by another product.");
        }
    }
}