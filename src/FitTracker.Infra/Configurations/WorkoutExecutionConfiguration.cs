using FitTracker.Domain.Entities.Workouts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitTracker.Infra.Configurations
{
    internal class WorkoutExecutionConfiguration : IEntityTypeConfiguration<WorkoutExecution>
    {
        public void Configure(EntityTypeBuilder<WorkoutExecution> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.StudentId).HasConversion(
                userId => userId.Value,
                value => new FitTracker.Domain.Entities.Users.UserId(value));

            builder.ToTable("WorkoutExecutions");
        }
    }
}
