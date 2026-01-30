namespace FitTracker.Application.Services.Users.Invite
{
    public record InviteStudentRequest(
        string Email,
        string Name,
        string Phone);
}
