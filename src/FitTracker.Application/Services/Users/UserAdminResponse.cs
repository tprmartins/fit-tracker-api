namespace FitTracker.Application.Services.Users
{
    public sealed record UserAdminResponse(
        Guid Id,
        string Name,
        string Email,
        string Document,
        string Phone,
        int Role,
        bool IsActive,
        DateTime? BlockedAt
    );
}
