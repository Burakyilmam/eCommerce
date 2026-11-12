using eCommerce.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace eCommerce.Infrastructure.Services;

public sealed class EmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public EmailService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task SendEmailConfirmationAsync(string toEmail, string confirmationLink, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(toEmail);
        ArgumentException.ThrowIfNullOrWhiteSpace(confirmationLink);

        var safeLink = WebUtility.HtmlEncode(confirmationLink);

        var html = $"""
            <!DOCTYPE html>
            <html lang="tr">
            <body style="font-family:Arial,sans-serif;line-height:1.6">
                <h2>HepsiBende'ye Hoş Geldiniz!</h2>
                <p>Merhaba,</p>
                <p>HepsiBende hesabınızı oluşturduğunuz için teşekkür ederiz.</p>
                <p>Üyeliğinizi tamamlamak için e-posta adresinizi doğrulamanız gerekiyor.</p>
                <p>
                    <a href="{safeLink}"
                       style="display:inline-block;background-color:#f27a1a;
                              color:#ffffff;padding:12px 24px;
                              text-decoration:none;border-radius:6px;">
                        E-posta Adresimi Doğrula
                    </a>
                </p>
                <p>Bu bağlantı 24 saat geçerlidir.</p>
                <p>Bu hesabı siz oluşturmadıysanız e-postayı dikkate almayabilirsiniz.</p>
            </body>
            </html>
            """;

        await SendEmailAsync(toEmail, "HepsiBende - E-posta Adresinizi Doğrulayın", html, cancellationToken);
    }

    public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(toEmail);
        ArgumentException.ThrowIfNullOrWhiteSpace(resetLink);

        var safeLink = WebUtility.HtmlEncode(resetLink);

        var html = $"""
            <!DOCTYPE html>
            <html lang="tr">
            <body style="font-family:Arial,sans-serif;line-height:1.6">
                <h2>Şifre Sıfırlama</h2>
                <p>Merhaba,</p>
                <p>Hesabınız için şifre sıfırlama talebi aldık.</p>
                <p>Yeni şifrenizi oluşturmak için aşağıdaki bağlantıya tıklayın:</p>
                <p>
                    <a href="{safeLink}">
                        Şifremi Sıfırla
                    </a>
                </p>
                <p>Bu bağlantı 30 dakika geçerlidir.</p>
                <p>Bu işlemi siz istemediyseniz e-postayı dikkate almayabilirsiniz.</p>
            </body>
            </html>
            """;

        await SendEmailAsync(toEmail, "Şifre Sıfırlama Talebi", html, cancellationToken);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string html, CancellationToken cancellationToken)
    {
        var apiKey = _configuration["Resend:ApiKey"] ?? throw new InvalidOperationException("Resend API key is not configured.");

        var fromEmail = _configuration["Resend:FromEmail"] ?? throw new InvalidOperationException("Resend sender email is not configured.");

        var payload = new
        {
            from = fromEmail,
            to = new[] { toEmail },
            subject,
            html
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Content = JsonContent.Create(payload);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Email could not be sent. HTTP status: {(int)response.StatusCode}");
        }
    }
}