
namespace eCommerce.Application.DTOs.Auth;

public sealed class RegistrationResponseDto
{
    public string Message { get; set; } = string.Empty;
    public bool RequiresEmailConfirmation { get; set; }
}
