using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Dashboard
{
    public sealed record GetAdminDashboardStatsQuery() : IQuery<AdminDashboardStatsResponse>;
}
