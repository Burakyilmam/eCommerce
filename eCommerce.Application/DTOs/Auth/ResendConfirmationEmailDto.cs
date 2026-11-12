
using System.ComponentModel.DataAnnotations;

namespace eCommerce.Application.DTOs.Auth;

public sealed class ResendConfirmationEmailDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
