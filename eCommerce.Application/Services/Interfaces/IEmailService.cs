
namespace eCommerce.Application.Services.Interfaces;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string toEmail, string resetLink, CancellationToken cancellationToken = default);
    Task SendEmailConfirmationAsync(string toEmail, string confirmationLink, CancellationToken cancellationToken = default);
}
