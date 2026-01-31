using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;
using FitTracker.Domain.ValueObjects;
using FitTracker.Domain.Entities.Users;

namespace FitTracker.Application.Services.Dashboard
{
    internal sealed class GetPersonalDashboardStatsQueryHandler : IQueryHandler<GetPersonalDashboardStatsQuery, PersonalDashboardStatsResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IWorkoutExecutionRepository _workoutExecutionRepository;
        private readonly IUserContext _userContext;

        public GetPersonalDashboardStatsQueryHandler(
            IUserRepository userRepository,
            IWorkoutRepository workoutRepository,
            IWorkoutExecutionRepository workoutExecutionRepository,
            IUserContext userContext)
        {
            _userRepository = userRepository;
            _workoutRepository = workoutRepository;
            _workoutExecutionRepository = workoutExecutionRepository;
            _userContext = userContext;
        }

        public async Task<Result<PersonalDashboardStatsResponse>> Handle(GetPersonalDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;

            // 1. Total Students
            var totalStudents = await _userRepository.CountByRoleAsync(UserRole.Student, cancellationToken);

            // 2. Active Plans: Workouts created by this Personal
            var activePlans = await _workoutRepository.CountByPersonalIdAsync(new UserId(userId), cancellationToken);

            // 3. Workouts Completed: Executions recorded
            var workoutsCompleted = await _workoutExecutionRepository.CountByPersonalIdAsync(new UserId(userId), cancellationToken);

            // Mocked Data for metrics we don't track yet
            var studentProgressChange = 0; // Need Weight/performance tracking logic

            return new PersonalDashboardStatsResponse(
                totalStudents,
                activePlans,
                workoutsCompleted,
                studentProgressChange
            );
        }
    }
}
