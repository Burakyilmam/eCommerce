using AutoMapper;
using eCommerce.Application.DTOs.Categories;
using eCommerce.Domain.Entities;

namespace eCommerce.Application.Mapping
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryDto>();

            CreateMap<CreateCategoryDto, Category>();

            CreateMap<UpdateCategoryDto, Category>();
        }
    }
}
