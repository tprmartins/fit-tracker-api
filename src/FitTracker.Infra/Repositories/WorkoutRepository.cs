using Microsoft.EntityFrameworkCore;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Entities.Workouts;
using FitTracker.Domain.Repositories;
using FitTracker.Infra.Context;

namespace FitTracker.Infra.Repositories
{
    public sealed class WorkoutRepository : IWorkoutRepository
    {
        private readonly DatabaseContext _dbContext;

        public WorkoutRepository(DatabaseContext dbContext) =>
            _dbContext = dbContext;

        public async Task<Workout?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
            await _dbContext
                .Set<Workout>()
                .Include(w => w.WorkoutDays)
                .ThenInclude(wd => wd.Exercises)
                .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

        public async Task<IEnumerable<Workout>> GetByStudentIdAsync(UserId studentId, CancellationToken cancellationToken) =>
            await _dbContext
                .Set<Workout>()
                .Where(w => w.StudentId == studentId)
                .Include(w => w.WorkoutDays)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Workout>> GetByPersonalIdAsync(UserId personalId, CancellationToken cancellationToken) =>
            await _dbContext
                .Set<Workout>()
                .Where(w => w.PersonalId == personalId)
                .ToListAsync(cancellationToken);

        public void Add(Workout workout) =>
            _dbContext.Set<Workout>().Add(workout);

        public void Update(Workout workout) =>
            _dbContext.Set<Workout>().Update(workout);

        public void Remove(Workout workout) =>
            _dbContext.Set<Workout>().Remove(workout);
    }
}
