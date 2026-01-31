using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.ValueObjects;

namespace FitTracker.Infra.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                userId => userId.Value,
                value => new UserId(value));

            builder.Property(c => c.BlockedBy).HasConversion(
                userId => userId != null ? userId.Value : (Guid?)null,
                value => value != null ? new UserId(value.Value) : null);

            builder.Property(c => c.Name).HasMaxLength(128).IsRequired();

            builder.Property(c => c.Email)
                .HasConversion(x => x.Value, v => Email.Create(v).Value)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(c => c.Password).HasMaxLength(256).IsRequired(false);

            builder
                .Property(x => x.Document)
                .HasConversion(x => x != null ? x.Value : null, v => v != null ? Document.Create(v).Value : null)
                .IsRequired(false);

            builder.Property(c => c.Phone).HasMaxLength(50);

            builder.Property(c => c.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(c => c.RegistrationToken)
                .HasMaxLength(128)
                .IsRequired(false);

            builder.Property(c => c.TokenExpiresAt)
                .IsRequired(false);

            builder.Property(c => c.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(c => c.DeletedAt)
                .IsRequired(false);

            builder.HasQueryFilter(u => !u.IsDeleted);

            builder.HasIndex(c => c.Document).IsUnique().HasFilter("\"Document\" IS NOT NULL");
            builder.HasIndex(c => c.Email).IsUnique();
        }
    }
}
