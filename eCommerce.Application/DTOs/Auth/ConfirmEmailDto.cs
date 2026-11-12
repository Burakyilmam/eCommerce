
using System.ComponentModel.DataAnnotations;

namespace eCommerce.Application.DTOs.Auth;

public sealed class ConfirmEmailDto
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string Token { get; set; } = string.Empty;
}
