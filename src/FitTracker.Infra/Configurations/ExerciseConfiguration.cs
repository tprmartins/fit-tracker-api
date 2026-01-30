using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FitTracker.Domain.Entities.Workouts;

namespace FitTracker.Infra.Configurations
{
    internal class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).HasMaxLength(128).IsRequired();
            builder.Property(e => e.Sets).HasMaxLength(50);
            builder.Property(e => e.Reps).HasMaxLength(50);
            builder.Property(e => e.Weight).HasMaxLength(50);
            builder.Property(e => e.VideoUrl).HasMaxLength(256);
        }
    }
}
