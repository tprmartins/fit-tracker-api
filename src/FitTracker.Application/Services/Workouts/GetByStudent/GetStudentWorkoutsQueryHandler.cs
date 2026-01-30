using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Workouts.GetByStudent
{
    internal sealed class GetStudentWorkoutsQueryHandler : IQueryHandler<GetStudentWorkoutsQuery, List<WorkoutResponse>>
    {
        private readonly IWorkoutRepository _workoutRepository;

        public GetStudentWorkoutsQueryHandler(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public async Task<Result<List<WorkoutResponse>>> Handle(GetStudentWorkoutsQuery request, CancellationToken cancellationToken)
        {
            var workouts = await _workoutRepository.GetByStudentIdAsync(new UserId(request.StudentId), cancellationToken);

            var response = workouts.Select(w => new WorkoutResponse(
                w.Id,
                w.Name,
                w.Description,
                w.DurationWeeks,
                w.FrequencyDaysPerWeek,
                w.CreatedAt,
                w.WorkoutDays.Select(wd => new WorkoutDayResponse(
                    wd.Id,
                    wd.Name,
                    wd.Exercises.Select(e => new ExerciseResponse(
                        e.Id,
                        e.Name,
                        e.Sets,
                        e.Reps,
                        e.Weight,
                        e.VideoUrl
                    )).ToList()
                )).ToList()
            )).ToList();

            return response;
        }
    }
}
