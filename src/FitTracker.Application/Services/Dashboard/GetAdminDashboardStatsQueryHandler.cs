using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Dashboard
{
    internal sealed class GetAdminDashboardStatsQueryHandler : IQueryHandler<GetAdminDashboardStatsQuery, AdminDashboardStatsResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IUserContext _userContext;

        public GetAdminDashboardStatsQueryHandler(
            IUserRepository userRepository,
            IWorkoutRepository workoutRepository,
            IUserContext userContext)
        {
            _userRepository = userRepository;
            _workoutRepository = workoutRepository;
            _userContext = userContext;
        }

        public async Task<Result<AdminDashboardStatsResponse>> Handle(GetAdminDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            if (_userContext.Role != UserRole.Admin)
            {
                return Result.Failure<AdminDashboardStatsResponse>(new Error("Auth.Unauthorized", "Only admins can access admin statistics."));
            }

            var totalUsers = await _userRepository.CountAllAsync(cancellationToken);
            var activeUsers = await _userRepository.CountActiveAsync(cancellationToken);
            var blockedUsers = await _userRepository.CountBlockedAsync(cancellationToken);

            var totalPersonals = await _userRepository.CountByRoleAsync(UserRole.Personal, cancellationToken);
            var totalStudents = await _userRepository.CountByRoleAsync(UserRole.Student, cancellationToken);

            var totalWorkouts = await _workoutRepository.CountAllAsync(cancellationToken);

            return new AdminDashboardStatsResponse(
                totalUsers,
                activeUsers,
                blockedUsers,
                totalPersonals,
                totalStudents,
                totalWorkouts
            );
        }
    }
}
