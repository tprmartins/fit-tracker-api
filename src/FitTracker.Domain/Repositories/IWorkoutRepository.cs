using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Entities.Workouts;

namespace FitTracker.Domain.Repositories
{
    public interface IWorkoutRepository
    {
        Task<Workout?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Workout>> GetByStudentIdAsync(UserId studentId, CancellationToken cancellationToken);
        Task<IEnumerable<Workout>> GetByPersonalIdAsync(UserId personalId, CancellationToken cancellationToken);
        Task<int> CountByPersonalIdAsync(UserId personalId, CancellationToken cancellationToken);
        Task<int> CountAllAsync(CancellationToken cancellationToken);
        void Add(Workout workout);
        void Update(Workout workout);
        void Remove(Workout workout);
    }
}
