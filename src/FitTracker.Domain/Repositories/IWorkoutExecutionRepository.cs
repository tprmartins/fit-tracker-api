using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Entities.Workouts;

namespace FitTracker.Domain.Repositories
{
    public interface IWorkoutExecutionRepository
    {
        Task<int> CountByPersonalIdAsync(UserId personalId, CancellationToken cancellationToken);
        Task<List<WorkoutExecutionDetail>> GetRecentByPersonalIdAsync(UserId personalId, int limit, CancellationToken cancellationToken);
        void Add(WorkoutExecution execution);
    }

    public record WorkoutExecutionDetail(Guid Id, string StudentName, string WorkoutName, DateTime CompletedAt);
}
