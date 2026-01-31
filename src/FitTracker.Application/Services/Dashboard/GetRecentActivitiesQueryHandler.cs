using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;
using FitTracker.Domain.ValueObjects;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Entities.Users;

namespace FitTracker.Application.Services.Dashboard
{
    internal sealed class GetRecentActivitiesQueryHandler : IQueryHandler<GetRecentActivitiesQuery, List<RecentActivityResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IWorkoutExecutionRepository _workoutExecutionRepository;
        private readonly IUserContext _userContext;

        public GetRecentActivitiesQueryHandler(
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

        public async Task<Result<List<RecentActivityResponse>>> Handle(GetRecentActivitiesQuery request, CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;
            var activities = new List<RecentActivityResponse>();

            // 1. Get recent students (invited or registered)
            // Note: Currently IUserRepository doesn't have a "GetRecent" method. 
            // I'll assume GetAllAsync and then filter/sort here for now, or just take top.
            // Ideally should be a repository method.
            var students = await _userRepository.GetByRoleAsync(UserRole.Student, cancellationToken);

            var recentStudents = students
                .OrderByDescending(u => u.CreatedAt)
                .Take(5);

            foreach (var student in recentStudents)
            {
                activities.Add(new RecentActivityResponse(
                    student.Id.Value.ToString(),
                    "Personal", // Action performed by the Personal (You)
                    "StudentInvited",
                    student.Status == UserStatus.PendingCompletion ? "invited_student" : "started_plan",
                    FormatTimeAgo(student.CreatedAt, out int count),
                    student.CreatedAt
                )
                { ActivityCount = count, TargetName = student.Name });
            }

            // 2. Get recent workouts created by this Personal
            var workouts = await _workoutRepository.GetByPersonalIdAsync(new UserId(userId), cancellationToken);
            var recentWorkouts = workouts
                .OrderByDescending(w => w.CreatedAt)
                .Take(5);

            foreach (var workout in recentWorkouts)
            {
                activities.Add(new RecentActivityResponse(
                    workout.Id.ToString(),
                    "Personal",
                    "WorkoutCreated",
                    "created_plan",
                    FormatTimeAgo(workout.CreatedAt, out int count),
                    workout.CreatedAt
                )
                { ActivityCount = count, TargetName = workout.Name });
            }

            // 3. Get recent workout executions for students of this Personal
            var executions = await _workoutExecutionRepository.GetRecentByPersonalIdAsync(new UserId(userId), 5, cancellationToken);

            foreach (var execution in executions)
            {
                activities.Add(new RecentActivityResponse(
                    execution.Id.ToString(),
                    execution.StudentName,
                    "WorkoutCompleted",
                    "completed_workout",
                    FormatTimeAgo(execution.CompletedAt, out int count),
                    execution.CompletedAt
                )
                { ActivityCount = count, TargetName = execution.WorkoutName });
            }

            return activities
                .OrderByDescending(a => a.Timestamp)
                .Take(10)
                .ToList();
        }

        private static string FormatTimeAgo(DateTime timestamp, out int count)
        {
            var diff = DateTime.UtcNow - timestamp;
            if (diff.TotalMinutes < 60)
            {
                count = (int)diff.TotalMinutes;
                return "time_now";
            }
            count = (int)diff.TotalHours;
            return "time_ago";
        }
    }
}
