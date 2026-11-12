using eCommerce.Application.DTOs.Auth;

namespace eCommerce.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegistrationResponseDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);
        Task ForgotPasswordAsync(ForgotPasswordDto dto, CancellationToken cancellationToken = default);
        Task ResetPasswordAsync(ResetPasswordDto dto, CancellationToken cancellationToken = default);
        Task<bool> ValidateResetTokenAsync(string userId, string token, CancellationToken cancellationToken = default);
        Task<bool> ConfirmEmailAsync(string userId, string token, CancellationToken cancellationToken = default);
        Task ResendConfirmationEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}