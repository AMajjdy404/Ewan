using Ewan.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ewan.Infrastructure.Data.Config
{
   

    public class ClientPasswordResetTokenConfiguration : IEntityTypeConfiguration<ClientPasswordResetToken>
    {
        public void Configure(EntityTypeBuilder<ClientPasswordResetToken> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TokenHash)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(x => x.CreatedByIpAddress)
                .HasMaxLength(100);

            builder.Property(x => x.ExpiresAt)
                .IsRequired();

            builder.Property(x => x.IsUsed)
                .HasDefaultValue(false);

            builder.HasIndex(x => x.TokenHash);

            builder.HasIndex(x => new { x.ClientId, x.IsUsed, x.ExpiresAt });
        }
    }
}
