using AutoMapper;
using eCommerce.Application.DTOs.Brand;
using eCommerce.Domain.Entities;

namespace eCommerce.Application.Mapping;

public class BrandMappingProfile : Profile
{
    public BrandMappingProfile()
    {
        CreateMap<Brand, BrandDto>();

        CreateMap<CreateBrandDto, Brand>();

        CreateMap<UpdateBrandDto, Brand>();
    }
}