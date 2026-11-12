using eCommerce.Application.DTOs.Auth;
using eCommerce.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RegistrationResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<RegistrationResponseDto>> Register([FromBody] RegisterDto dto, CancellationToken cancellationToken)
    {
        var response = await _authService.RegisterAsync(dto, cancellationToken);
        return Ok(response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        var response = await _authService.LoginAsync(dto, cancellationToken);
        return Ok(response);
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto, CancellationToken cancellationToken)
    {
        await _authService.ForgotPasswordAsync(dto, cancellationToken);

        return Ok(new
        {
            message = "If an account with this email address exists, a password reset link will be sent."
        });
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto, CancellationToken cancellationToken)
    {
        await _authService.ResetPasswordAsync(dto, cancellationToken);

        return Ok(new
        {
            message = "Your password has been successfully updated."
        });
    }

    [HttpPost("validate-reset-token")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateResetToken([FromBody] ValidateResetTokenDto dto, CancellationToken cancellationToken)
    {
        var isValid = await _authService.ValidateResetTokenAsync(dto.UserId, dto.Token, cancellationToken);
        return Ok(new { isValid });
    }

    [HttpPost("confirm-email")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto dto, CancellationToken cancellationToken)
    {
        var isConfirmed = await _authService.ConfirmEmailAsync(dto.UserId, dto.Token, cancellationToken);

        if (!isConfirmed)
        {
            return BadRequest(new
            {
                message = "Invalid or expired email confirmation link."
            });
        }

        return Ok(new
        {
            message = "Your email address has been successfully verified."
        });
    }

    [HttpPost("resend-confirmation")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailDto dto, CancellationToken cancellationToken)
    {
        await _authService.ResendConfirmationEmailAsync(dto.Email, cancellationToken);

        return Ok(new
        {
            message = "If your account requires email verification, a confirmation link will be sent."
        });
    }
}