using FitTracker.Domain.Entities.Users;

namespace FitTracker.Domain.Entities.Workouts
{
    public sealed class WorkoutExecution
    {
        public WorkoutExecution(Guid id, Guid workoutId, UserId studentId)
        {
            Id = id;
            WorkoutId = workoutId;
            StudentId = studentId;
            CompletedAt = DateTime.UtcNow;
        }

        private WorkoutExecution()
        {
            StudentId = null!;
        }

        public Guid Id { get; private set; }
        public Guid WorkoutId { get; private set; }
        public UserId StudentId { get; private set; }
        public DateTime CompletedAt { get; private set; }
    }
}
