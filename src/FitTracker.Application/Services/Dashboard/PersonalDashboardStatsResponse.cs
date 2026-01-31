namespace FitTracker.Application.Services.Dashboard
{
    public sealed record PersonalDashboardStatsResponse(
        int TotalStudents,
        int ActivePlans,
        int WorkoutsCompleted,
        int StudentProgressChange
    );
}
