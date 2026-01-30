using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Entities.Workouts;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Workouts.Create
{
    internal sealed class CreateWorkoutCommandHandler : ICommandHandler<CreateWorkoutCommand, Guid>
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWorkoutCommandHandler(IWorkoutRepository workoutRepository, IUnitOfWork unitOfWork)
        {
            _workoutRepository = workoutRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
        {
            var workout = new Workout(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                request.DurationWeeks,
                request.FrequencyDaysPerWeek,
                new UserId(request.StudentId),
                new UserId(request.PersonalId)
            );

            foreach (var dayRequest in request.WorkoutDays)
            {
                var workoutDay = new WorkoutDay(Guid.NewGuid(), dayRequest.Name, workout.Id);
                
                foreach (var exerciseRequest in dayRequest.Exercises)
                {
                    var exercise = new Exercise(
                        Guid.NewGuid(),
                        exerciseRequest.Name,
                        exerciseRequest.Sets,
                        exerciseRequest.Reps,
                        exerciseRequest.Weight,
                        exerciseRequest.VideoUrl,
                        workoutDay.Id
                    );
                    workoutDay.Exercises.Add(exercise);
                }
                
                workout.WorkoutDays.Add(workoutDay);
            }

            _workoutRepository.Add(workout);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return workout.Id;
        }
    }
}
