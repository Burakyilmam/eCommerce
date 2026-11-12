
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Infrastructure.Seeders;

public static class IdentityRoleSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        string[] roles =
        {
            "Customer",
            "Seller",
            "Admin"
        };

        foreach (var roleName in roles)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                continue;
            }

            var result = await roleManager.CreateAsync(new ApplicationRole
            {
                Name = roleName
            });

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                throw new InvalidOperationException($"Failed to create role '{roleName}': {errors}");
            }
        }
    }
}
