
using eCommerce.Domain.Entities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eCommerce.Infrastructure.Services;

public sealed class EmailConfirmationTokenProvider
    : DataProtectorTokenProvider<ApplicationUser>
{
    public EmailConfirmationTokenProvider(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<EmailConfirmationTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<ApplicationUser>> logger) : base(
            dataProtectionProvider,
            Microsoft.Extensions.Options.Options.Create(new DataProtectionTokenProviderOptions
            {
                Name = "EmailConfirmation",
                TokenLifespan = options.Value.TokenLifespan
            }),
            logger)
    {
    }
}
