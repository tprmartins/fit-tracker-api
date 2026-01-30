using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Workouts.Create
{
    public record CreateWorkoutCommand(
        string Name, 
        string Description, 
        int DurationWeeks, 
        int FrequencyDaysPerWeek, 
        Guid StudentId, 
        Guid PersonalId,
        List<WorkoutDayRequest> WorkoutDays) : ICommand<Guid>;

    public record WorkoutDayRequest(string Name, List<ExerciseRequest> Exercises);

    public record ExerciseRequest(string Name, string Sets, string Reps, string Weight, string VideoUrl);
}
