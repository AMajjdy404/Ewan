using Ewan.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ewan.Infrastructure.Data.Config
{
    public class PropertyRatingConfiguration : IEntityTypeConfiguration<PropertyRating>
    {
        public void Configure(EntityTypeBuilder<PropertyRating> builder)
        {
            builder.Property(x => x.Comment)
                .HasMaxLength(1000);

            builder.HasIndex(x => new { x.ClientId, x.PropertyId })
                .IsUnique();

            builder.HasOne(x => x.Client)
                .WithMany(x => x.Ratings)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Property)
                .WithMany(x => x.Ratings)
                .HasForeignKey(x => x.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
