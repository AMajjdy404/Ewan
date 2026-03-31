using Ewan.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ewan.Infrastructure.Data.Config
{
    public class PropertyGroupConfiguration : IEntityTypeConfiguration<PropertyGroup>
    {
        public void Configure(EntityTypeBuilder<PropertyGroup> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
