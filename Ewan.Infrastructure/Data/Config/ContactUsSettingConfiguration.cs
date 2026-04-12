using Ewan.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ewan.Infrastructure.Data.Config
{
    public class ContactUsSettingConfiguration : IEntityTypeConfiguration<ContactUsSetting>
    {
        public void Configure(EntityTypeBuilder<ContactUsSetting> builder)
        {
            builder.Property(x => x.SupportNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.WhatsappNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256);
        }
    }
}
