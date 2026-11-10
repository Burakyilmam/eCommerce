using eCommerce.Application.Services.Interfaces;
using eCommerce.Domain.Entities;

namespace eCommerce.Application.Services.Implementations;

public class ProductService : GenericService<Product>, IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork) : base(productRepository, unitOfWork)
    {
        _productRepository = productRepository;
    }

    public async Task<Product?> GetProductBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _productRepository.GetProductBySlugAsync(slug, cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> GetNewestAsync(CancellationToken cancellationToken = default)
    {
        return await _productRepository.GetNewestProductsAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> GetMostViewedAsync(CancellationToken cancellationToken = default)
    {
        return await _productRepository.GetMostViewedProductsAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> GetProductsWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _productRepository.GetProductsWithDetailsAsync(cancellationToken);
    }

    public Task<Product?> GetProductDetailsAsync(int productId, CancellationToken cancellationToken = default)
    {
        return _productRepository.GetProductDetailsAsync(productId, cancellationToken);
    }
}