using eCommerce.Application.DTOs.Auth;
using eCommerce.Application.Exceptions;
using eCommerce.Application.Services.Interfaces;
using eCommerce.Domain.Entities;
using eCommerce.Domain.Enums;
using eCommerce.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eCommerce.Infrastructure.Services;

public sealed class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        AppDbContext context,
        IConfiguration configuration,
        IEmailService emailService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<RegistrationResponseDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);
        cancellationToken.ThrowIfCancellationRequested();

        var email = dto.Email.Trim();
        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser is not null)
        {
            throw new ConflictException("This email address is already in use.");
        }

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = false,
            UserType = UserType.Customer,
            IsActive = true
        };

        var createResult = await _userManager.CreateAsync(user, dto.Password);

        if (!createResult.Succeeded)
        {
            throw new ArgumentException(string.Join(" ", createResult.Errors.Select(x => x.Description)));
        }

        if (!await _roleManager.RoleExistsAsync("Customer"))
        {
            throw new InvalidOperationException("The 'Customer' role is not defined in the system.");
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "Customer");

        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException("Failed to assign role to the user.");
        }

        var profile = new UserProfile
        {
            UserId = user.Id,
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim()
        };

        await _context.UserProfiles.AddAsync(profile, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        await SendConfirmationEmailAsync(user, cancellationToken);

        return new RegistrationResponseDto
        {
            Message = "Please verify your email address to activate your account.",
            RequiresEmailConfirmation = true
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);
        cancellationToken.ThrowIfCancellationRequested();

        var user = await _userManager.FindByEmailAsync(dto.Email.Trim());

        if (user is null || !user.IsActive || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        if (!user.EmailConfirmed)
        {
            throw new UnauthorizedAccessException("Please verify your email address before logging in.");
        }

        var profile = await _context.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);

        return await GenerateAuthResponseAsync(user, profile, cancellationToken);
    }

    private async Task<AuthResponseDto> GenerateAuthResponseAsync(
        ApplicationUser user,
        UserProfile? profile,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var jwtSection = _configuration.GetSection("Jwt");

        var issuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
        var audience = jwtSection["Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");
        var key = jwtSection["Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");

        if (Encoding.UTF8.GetByteCount(key) < 32)
        {
            throw new InvalidOperationException("The JWT key must be at least 32 bytes long.");
        }

        if (!int.TryParse(jwtSection["ExpirationMinutes"], out var expirationMinutes) || expirationMinutes <= 0)
        {
            throw new InvalidOperationException("Invalid JWT expiration period.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, user.UserName ?? string.Empty)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AuthResponseDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt,
            UserId = user.Id,
            Email = user.Email ?? string.Empty,
            FirstName = profile?.FirstName ?? string.Empty,
            LastName = profile?.LastName ?? string.Empty,
            Roles = roles.ToArray()
        };
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDto dto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);
        cancellationToken.ThrowIfCancellationRequested();

        var user = await _userManager.FindByEmailAsync(dto.Email.Trim());

        if (user is null || !user.IsActive || user.Email is null)
        {
            return;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var frontendUrl = _configuration["Frontend:BaseUrl"] ?? 
                          throw new InvalidOperationException("Frontend BaseUrl is not configured.");

        var resetLink = $"{frontendUrl.TrimEnd('/')}/reset-password" +
                        $"?userId={Uri.EscapeDataString(user.Id)}" +
                        $"&token={Uri.EscapeDataString(token)}";

        await _emailService.SendPasswordResetEmailAsync(user.Email, resetLink, cancellationToken);
    }

    public async Task ResetPasswordAsync(ResetPasswordDto dto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);
        cancellationToken.ThrowIfCancellationRequested();

        if (dto.NewPassword != dto.ConfirmPassword)
        {
            throw new ArgumentException("Passwords do not match.");
        }

        var user = await _userManager.FindByIdAsync(dto.UserId);

        if (user is null || !user.IsActive)
        {
            throw new ArgumentException("Invalid or expired password reset link.");
        }

        var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

        if (!result.Succeeded)
        {
            throw new ArgumentException("Invalid reset token or password does not meet security requirements.");
        }
    }

    public async Task<bool> ValidateResetTokenAsync(string userId, string token, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null || !user.IsActive)
        {
            return false;
        }

        return await _userManager.VerifyUserTokenAsync(
            user,
            _userManager.Options.Tokens.PasswordResetTokenProvider,
            UserManager<ApplicationUser>.ResetPasswordTokenPurpose,
            token);
    }

    private async Task SendConfirmationEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new InvalidOperationException("User email is missing.");
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var frontendUrl = _configuration["Frontend:BaseUrl"] ?? 
                          throw new InvalidOperationException("Frontend BaseUrl is not configured.");

        var confirmationLink = $"{frontendUrl.TrimEnd('/')}/confirm-email" +
                               $"?userId={Uri.EscapeDataString(user.Id)}" +
                               $"&token={Uri.EscapeDataString(token)}";

        await _emailService.SendEmailConfirmationAsync(user.Email, confirmationLink, cancellationToken);
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string token, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null || !user.IsActive || user.EmailConfirmed)
        {
            return false;
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);

        return result.Succeeded;
    }

    public async Task ResendConfirmationEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(email))
        {
            return;
        }

        var user = await _userManager.FindByEmailAsync(email.Trim());

        if (user is null || !user.IsActive || user.EmailConfirmed)
        {
            return;
        }

        await SendConfirmationEmailAsync(user, cancellationToken);
    }
}