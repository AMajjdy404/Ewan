using Ewan.Core.Models;
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

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.OwnerPhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.OwnerPasswordHash)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.BookingMode)
                .IsRequired();

            builder.Property(x => x.PropertyType)
                .IsRequired();

            builder.HasIndex(x => x.OwnerPhoneNumber)
                .IsUnique();

            builder.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Location)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.PricePerNight)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.PricePerHour)
                .HasColumnType("decimal(18,2)");
        }
    }
}
