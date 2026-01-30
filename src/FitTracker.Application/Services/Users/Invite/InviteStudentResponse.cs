namespace FitTracker.Application.Services.Users.Invite
{
    public record InviteStudentResponse(
        Guid Id,
        string Email,
        string Name,
        string RegistrationToken);
}
