using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Application.Services.Workouts.GetByStudent;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Workouts.GetByPersonal
{
    internal sealed class GetPersonalWorkoutsQueryHandler : IQueryHandler<GetPersonalWorkoutsQuery, List<WorkoutResponse>>
    {
        private readonly IWorkoutRepository _workoutRepository;

        public GetPersonalWorkoutsQueryHandler(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public async Task<Result<List<WorkoutResponse>>> Handle(GetPersonalWorkoutsQuery request, CancellationToken cancellationToken)
        {
            var workouts = await _workoutRepository.GetByPersonalIdAsync(new UserId(request.PersonalId), cancellationToken);

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
