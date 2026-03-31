using Ewan.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ewan.Infrastructure.Data.Config
{
    public class PropertyFacilityConfiguration : IEntityTypeConfiguration<PropertyFacility>
    {
        public void Configure(EntityTypeBuilder<PropertyFacility> builder)
        {
            builder.HasKey(x => new { x.PropertyId, x.FacilityId });

            builder.HasOne(x => x.Property)
                .WithMany(x => x.PropertyFacilities)
                .HasForeignKey(x => x.PropertyId);

            builder.HasOne(x => x.Facility)
                .WithMany(x => x.PropertyFacilities)
                .HasForeignKey(x => x.FacilityId);
        }
    }
}
