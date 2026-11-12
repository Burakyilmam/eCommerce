
using System.ComponentModel.DataAnnotations;

namespace eCommerce.Application.DTOs.Auth;

public sealed class ResetPasswordDto
{
    [Required(ErrorMessage = "User ID is required.")]
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Reset token is required.")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password confirmation is required.")]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
