using FitTracker.Domain.Entities.RefreshTokens;
using FitTracker.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitTracker.Infra.Configurations
{
    internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .HasConversion(id => id.Value, value => new UserId(value))
                .IsRequired();

            builder.Property(x => x.TokenHash)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.ExpiresAt)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.IsUsed)
                .IsRequired();

            builder.Property(x => x.IsRevoked)
                .IsRequired();

            builder.HasIndex(x => x.TokenHash).IsUnique();
            builder.HasIndex(x => x.UserId);
        }
    }
}
