using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Workouts.Execute
{
    public sealed record ExecuteWorkoutCommand(Guid WorkoutId) : ICommand;
}
