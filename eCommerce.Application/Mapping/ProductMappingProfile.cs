
using AutoMapper;
using eCommerce.Application.DTOs.Products;
using eCommerce.Domain.Entities;

namespace eCommerce.Application.Mapping;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(
                dest => dest.SellerListingId,
                opt => opt.MapFrom(src =>
                    src.Variants
                        .Where(v => v.IsActive)
                        .SelectMany(v => v.SellerListings)
                        .Where(l => l.IsActive && l.StockQuantity > 0)
                        .OrderBy(l => l.Price)
                        .Select(l => (int?)l.Id)
                        .FirstOrDefault()
                )
            )
            .ForMember(
                dest => dest.ProductVariantId,
                opt => opt.MapFrom(src =>
                    src.Variants
                        .Where(v => v.IsActive)
                        .SelectMany(v => v.SellerListings)
                        .Where(l => l.IsActive && l.StockQuantity > 0)
                        .OrderBy(l => l.Price)
                        .Select(l => (int?)l.ProductVariantId)
                        .FirstOrDefault()
                )
            )
            .ForMember(
                dest => dest.Price,
                opt => opt.MapFrom(src =>
                    src.Variants
                        .Where(v => v.IsActive)
                        .SelectMany(v => v.SellerListings)
                        .Where(l => l.IsActive && l.StockQuantity > 0)
                        .OrderBy(l => l.Price)
                        .Select(l => (decimal?)l.Price)
                        .FirstOrDefault()
                )
            )
            .ForMember(
                dest => dest.OldPrice,
                opt => opt.MapFrom(src =>
                    src.Variants
                        .Where(v => v.IsActive)
                        .SelectMany(v => v.SellerListings)
                        .Where(l => l.IsActive && l.StockQuantity > 0)
                        .OrderBy(l => l.Price)
                        .Select(l => l.CompareAtPrice)
                        .FirstOrDefault()
                )
            )
            .ForMember(
                dest => dest.StockQuantity,
                opt => opt.MapFrom(src =>
                    src.Variants
                        .Where(v => v.IsActive)
                        .SelectMany(v => v.SellerListings)
                        .Where(l => l.IsActive && l.StockQuantity > 0)
                        .OrderBy(l => l.Price)
                        .Select(l => (int?)l.StockQuantity)
                        .FirstOrDefault()
                )
            )
            .ForMember(
                dest => dest.CurrencyCode,
                opt => opt.MapFrom(src =>
                    src.Variants
                        .Where(v => v.IsActive)
                        .SelectMany(v => v.SellerListings)
                        .Where(l => l.IsActive && l.StockQuantity > 0)
                        .OrderBy(l => l.Price)
                        .Select(l => l.CurrencyCode)
                        .FirstOrDefault()
                )
            )
            .ForMember(
                dest => dest.Image,
                opt => opt.MapFrom(src =>
                    src.Images
                        .OrderByDescending(i => i.IsMain)
                        .ThenBy(i => i.DisplayOrder)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault()
                )
            );

        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
    }
}
