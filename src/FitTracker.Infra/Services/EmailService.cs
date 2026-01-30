using FitTracker.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace FitTracker.Infra.Services
{
    public sealed class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public Task SendInvitationEmailAsync(string email, string name, string token, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending invitation email to {Email} ({Name}) with token {Token}", email, name, token);
            // In a real implementation, you would use an email provider like SendGrid, Mailtrap, etc.
            return Task.CompletedTask;
        }
    }
}
