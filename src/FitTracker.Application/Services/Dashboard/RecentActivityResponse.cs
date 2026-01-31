namespace FitTracker.Application.Services.Dashboard
{
    public sealed record RecentActivityResponse(
        string Id,
        string UserName,
        string Type,
        string Description,
        string TimeAgo,
        DateTime Timestamp
    )
    {
        public int ActivityCount { get; init; }
        public string? TargetName { get; init; }
    }
}
