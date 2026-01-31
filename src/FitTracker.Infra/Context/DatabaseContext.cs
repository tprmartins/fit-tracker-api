using Microsoft.EntityFrameworkCore;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Entities.Workouts;
using FitTracker.Domain.Entities.RefreshTokens;

namespace FitTracker.Infra.Context
{
    public sealed class DatabaseContext : DbContext
    {
        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=FitTracker;Username=doadmin;Password=1234567890;Pooling=true;");
        }

        //entities
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Workout> Workouts { get; set; } = default!;
        public DbSet<WorkoutDay> WorkoutDays { get; set; } = default!;
        public DbSet<Exercise> Exercises { get; set; } = default!;
        public DbSet<WorkoutExecution> WorkoutExecutions { get; set; } = default!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }

}
