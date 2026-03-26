using Ewan.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ewan.Infrastructure.Data.Config
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TokenHash)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(x => x.OwnerId)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(x => x.UserType)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.DeviceInfo)
                .HasMaxLength(500);

            builder.Property(x => x.DeviceId)
                .HasMaxLength(200);

            builder.Property(x => x.RemoteIpAddress)
                .HasMaxLength(100);

            builder.Property(x => x.RevokedByIpAddress)
                .HasMaxLength(100);

            builder.Property(x => x.ReplacedByTokenHash)
                .HasMaxLength(128);

            builder.Property(x => x.CompromisedReason)
                .HasMaxLength(300);

            builder.HasIndex(x => x.TokenHash)
                .IsUnique();

            builder.HasIndex(x => new { x.OwnerId, x.UserType });

            builder.HasIndex(x => new { x.OwnerId, x.UserType, x.DeviceId });
        }
    }
}
