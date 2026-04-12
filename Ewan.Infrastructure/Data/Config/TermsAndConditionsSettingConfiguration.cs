using Ewan.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ewan.Infrastructure.Data.Config
{
    public class TermsAndConditionsSettingConfiguration : IEntityTypeConfiguration<TermsAndConditionsSetting>
    {
        public void Configure(EntityTypeBuilder<TermsAndConditionsSetting> builder)
        {
            builder.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(10000);
        }
    }
}
