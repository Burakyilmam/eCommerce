using eCommerce.Application.Mapping;
using eCommerce.Application.Services.Implementations;
using eCommerce.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(BrandMappingProfile));

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBrandService, BrandService>();

        return services;
    }
}