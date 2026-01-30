using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Workouts.GetByStudent
{
    public record GetStudentWorkoutsQuery(Guid StudentId) : IQuery<List<WorkoutResponse>>;

    public record WorkoutResponse(
        Guid Id, 
        string Name, 
        string Description, 
        int DurationWeeks, 
        int FrequencyDaysPerWeek, 
        DateTime CreatedAt,
        List<WorkoutDayResponse> WorkoutDays);

    public record WorkoutDayResponse(Guid Id, string Name, List<ExerciseResponse> Exercises);

    public record ExerciseResponse(Guid Id, string Name, string Sets, string Reps, string Weight, string VideoUrl);
}
