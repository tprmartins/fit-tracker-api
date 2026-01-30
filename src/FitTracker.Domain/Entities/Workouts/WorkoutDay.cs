namespace FitTracker.Domain.Entities.Workouts
{
    public class WorkoutDay
    {
        public WorkoutDay(Guid id, string name, Guid workoutId)
        {
            Id = id;
            Name = name;
            WorkoutId = workoutId;
            Exercises = new List<Exercise>();
        }

        private WorkoutDay() 
        { 
            Name = string.Empty;
            Exercises = new List<Exercise>();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Guid WorkoutId { get; private set; }
        public ICollection<Exercise> Exercises { get; private set; }
    }
}
