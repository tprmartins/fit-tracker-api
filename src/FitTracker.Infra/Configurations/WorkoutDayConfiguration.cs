using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FitTracker.Domain.Entities.Workouts;

namespace FitTracker.Infra.Configurations
{
    internal class WorkoutDayConfiguration : IEntityTypeConfiguration<WorkoutDay>
    {
        public void Configure(EntityTypeBuilder<WorkoutDay> builder)
        {
            builder.HasKey(wd => wd.Id);

            builder.Property(wd => wd.Name).HasMaxLength(128).IsRequired();

            builder.HasMany(wd => wd.Exercises)
                .WithOne()
                .HasForeignKey(e => e.WorkoutDayId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
