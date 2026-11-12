
using System.ComponentModel.DataAnnotations;

namespace eCommerce.Application.DTOs.Auth;

public sealed class ForgotPasswordDto
{
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;
}
