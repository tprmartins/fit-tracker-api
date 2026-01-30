namespace FitTracker.Application.Abstractions
{
    public interface IEmailService
    {
        Task SendInvitationEmailAsync(string email, string name, string token, CancellationToken cancellationToken = default);
    }
}
