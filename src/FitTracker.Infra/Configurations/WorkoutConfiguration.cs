using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FitTracker.Domain.Entities.Workouts;
using FitTracker.Domain.Entities.Users;

namespace FitTracker.Infra.Configurations
{
    internal class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
    {
        public void Configure(EntityTypeBuilder<Workout> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name).HasMaxLength(128).IsRequired();
            builder.Property(w => w.Description).HasMaxLength(512);

            builder.Property(w => w.StudentId).HasConversion(
                id => id.Value,
                value => new UserId(value));

            builder.Property(w => w.PersonalId).HasConversion(
                id => id.Value,
                value => new UserId(value));

            builder.HasMany(w => w.WorkoutDays)
                .WithOne()
                .HasForeignKey(wd => wd.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
