
using eCommerce.Application;
using eCommerce.Application.Abstractions;
using eCommerce.Application.Services.Interfaces;
using eCommerce.Infrastructure.Contexts;
using eCommerce.Infrastructure.Repositories;
using eCommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // PostgreSQL
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));

        // ASP.NET Core Identity
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;

            // E-posta doğrulama için ayrı token provider
            options.Tokens.EmailConfirmationTokenProvider =
                "EmailConfirmation";
        })
        .AddRoles<ApplicationRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders()
        .AddTokenProvider<EmailConfirmationTokenProvider>("EmailConfirmation");

        // Varsayılan token provider: 30 dakika
        // Şifre sıfırlama bu provider'ı kullanır.
        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(30);
        });

        // E-posta doğrulama: 24 saat
        services.Configure<EmailConfirmationTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(24);
        });

        // Generic Repository
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Specific Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<ISliderRepository, SliderRepository>();

        // Authentication
        services.AddScoped<IAuthService, AuthService>();
        services.AddHttpClient<IEmailService, EmailService>();

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}

// E-posta doğrulama token'ı için özel ayarlar
public sealed class EmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
{

}
