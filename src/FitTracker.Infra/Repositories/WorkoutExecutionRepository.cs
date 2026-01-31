using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Entities.Workouts;
using FitTracker.Domain.Repositories;
using FitTracker.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infra.Repositories
{
    internal sealed class WorkoutExecutionRepository : IWorkoutExecutionRepository
    {
        private readonly DatabaseContext _dbContext;

        public WorkoutExecutionRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CountByPersonalIdAsync(UserId personalId, CancellationToken cancellationToken)
        {
            // Join with Workouts to filter by PersonalId
            return await _dbContext.WorkoutExecutions
                .Join(_dbContext.Workouts,
                    exe => exe.WorkoutId,
                    w => w.Id,
                    (exe, w) => new { exe, w })
                .Where(x => x.w.PersonalId == personalId)
                .CountAsync(cancellationToken);
        }

        public async Task<List<WorkoutExecutionDetail>> GetRecentByPersonalIdAsync(UserId personalId, int limit, CancellationToken cancellationToken)
        {
            var executions = await _dbContext.WorkoutExecutions
                .Join(_dbContext.Workouts,
                    exe => exe.WorkoutId,
                    w => w.Id,
                    (exe, w) => new { exe, w })
                .Where(x => x.w.PersonalId == personalId)
                .OrderByDescending(x => x.exe.CompletedAt)
                .Take(limit)
                .Join(_dbContext.Users,
                    x => x.exe.StudentId,
                    u => u.Id,
                    (x, u) => new WorkoutExecutionDetail(
                        x.exe.Id,
                        u.Name,
                        x.w.Name,
                        x.exe.CompletedAt
                    ))
                .ToListAsync(cancellationToken);

            return executions;
        }

        public void Add(WorkoutExecution execution)
        {
            _dbContext.WorkoutExecutions.Add(execution);
        }
    }
}
