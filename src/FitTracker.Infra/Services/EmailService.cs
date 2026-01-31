using FitTracker.Application.Abstractions;
using FitTracker.Infra.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace FitTracker.Infra.Services
{
    public sealed class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailOptions _emailOptions;

        public EmailService(ILogger<EmailService> logger, IOptions<EmailOptions> emailOptions)
        {
            _logger = logger;
            _emailOptions = emailOptions.Value;
        }

        public async Task SendInvitationEmailAsync(string email, string name, string token, CancellationToken cancellationToken = default)
        {
            var registrationLink = $"{_emailOptions.BaseUrl}/complete-registration?token={token}";

            var subject = "Bem-vindo ao FitTracker! Complete seu cadastro";

            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e2e8f0; border-radius: 12px;'>
                    <div style='text-align: center; margin-bottom: 24px;'>
                        <h2 style='color: #2563eb; margin-bottom: 8px;'>FitTracker</h2>
                        <p style='color: #64748b; font-size: 14px;'>Sua jornada fitness começa aqui.</p>
                    </div>
                    
                    <p style='color: #1e293b; font-size: 16px; line-height: 1.5;'>Olá, <strong>{name}</strong>!</p>
                    
                    <p style='color: #475569; font-size: 16px; line-height: 1.5;'>
                        Seu Personal Trainer acaba de convidar você para participar da nossa plataforma. 
                        Com o FitTracker, você poderá acompanhar seus treinos, evolução física e manter o foco nos seus objetivos.
                    </p>
                    
                    <div style='text-align: center; margin: 32px 0;'>
                        <a href='{registrationLink}' 
                           style='background: linear-gradient(to right, #2563eb, #4f46e5); color: white; padding: 14px 28px; text-decoration: none; border-radius: 8px; font-weight: bold; display: inline-block; box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);'>
                            Completar Meu Cadastro
                        </a>
                    </div>
                    
                    <p style='color: #64748b; font-size: 14px; line-height: 1.5;'>
                        Se o botão acima não funcionar, copie e cole o link abaixo no seu navegador:<br>
                        <a href='{registrationLink}' style='color: #2563eb;'>{registrationLink}</a>
                    </p>
                    
                    <hr style='border: 0; border-top: 1px solid #e2e8f0; margin: 32px 0;'>
                    
                    <p style='color: #94a3b8; font-size: 12px; text-align: center;'>
                        &copy; 2026 FitTracker. Todos os direitos reservados.
                    </p>
                </div>";

            await SendEmailAsync(email, subject, body, cancellationToken);
        }

        private async Task SendEmailAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_emailOptions.FromName, _emailOptions.FromEmail));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = htmlBody };

                using var smtp = new SmtpClient();

                var secureSocketOptions = _emailOptions.EnableSsl
                    ? SecureSocketOptions.StartTls
                    : SecureSocketOptions.None;

                // For testing/development, often 587 uses StartTls, 465 uses SslOnConnect
                if (_emailOptions.SmtpPort == 465)
                {
                    secureSocketOptions = SecureSocketOptions.SslOnConnect;
                }

                await smtp.ConnectAsync(_emailOptions.SmtpServer, _emailOptions.SmtpPort, secureSocketOptions, cancellationToken);

                if (!string.IsNullOrEmpty(_emailOptions.SmtpUsername))
                {
                    await smtp.AuthenticateAsync(_emailOptions.SmtpUsername, _emailOptions.SmtpPassword, cancellationToken);
                }

                await smtp.SendAsync(email, cancellationToken);
                await smtp.DisconnectAsync(true, cancellationToken);

                _logger.LogInformation("Email sent successfully to {To}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", to);
                // We don't want to break the registration flow if email fails in some systems, 
                // but for business critical app usually we might want to rethrow or track this.
                // For now, just logging.
            }
        }
    }
}
