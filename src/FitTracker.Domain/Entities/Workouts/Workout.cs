using FitTracker.Domain.Entities.Users;

namespace FitTracker.Domain.Entities.Workouts
{
    public class Workout
    {
        public Workout(Guid id, string name, string description, int durationWeeks, int frequencyDaysPerWeek, UserId studentId, UserId personalId)
        {
            Id = id;
            Name = name;
            Description = description;
            DurationWeeks = durationWeeks;
            FrequencyDaysPerWeek = frequencyDaysPerWeek;
            StudentId = studentId;
            PersonalId = personalId;
            CreatedAt = DateTime.UtcNow;
            WorkoutDays = new List<WorkoutDay>();
        }

        private Workout() 
        { 
            Name = string.Empty;
            Description = string.Empty;
            StudentId = null!;
            PersonalId = null!;
            WorkoutDays = new List<WorkoutDay>();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int DurationWeeks { get; private set; }
        public int FrequencyDaysPerWeek { get; private set; }
        public UserId StudentId { get; private set; }
        public UserId PersonalId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public ICollection<WorkoutDay> WorkoutDays { get; private set; }
    }
}
