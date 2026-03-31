using Ewan.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ewan.Infrastructure.Data.Config
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Location)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.PricePerNight)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Group)
                .WithMany(x => x.Properties)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
