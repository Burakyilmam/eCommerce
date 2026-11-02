using eCommerce.Domain.Entities;
using eCommerce.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace eCommerce.Infrastructure
{
    public class ApplicationUser : IdentityUser
    {
        public UserType UserType { get; set; } = UserType.Customer;

        public bool IsActive { get; set; } = true;

        public UserProfile? Profile { get; set; }
    }
}