namespace FitTracker.Domain.Entities.Workouts
{
    public class Exercise
    {
        public Exercise(Guid id, string name, string sets, string reps, string weight, string videoUrl, Guid workoutDayId)
        {
            Id = id;
            Name = name;
            Sets = sets;
            Reps = reps;
            Weight = weight;
            VideoUrl = videoUrl;
            WorkoutDayId = workoutDayId;
        }

        private Exercise() 
        { 
            Name = string.Empty;
            Sets = string.Empty;
            Reps = string.Empty;
            Weight = string.Empty;
            VideoUrl = string.Empty;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Sets { get; private set; }
        public string Reps { get; private set; }
        public string Weight { get; private set; }
        public string VideoUrl { get; private set; }
        public Guid WorkoutDayId { get; private set; }
    }
}
