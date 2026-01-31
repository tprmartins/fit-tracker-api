namespace FitTracker.Application.Services.Dashboard
{
    public sealed record AdminDashboardStatsResponse(
        int TotalUsers,
        int ActiveUsers,
        int BlockedUsers,
        int TotalPersonals,
        int TotalStudents,
        int TotalWorkouts
    );
}
