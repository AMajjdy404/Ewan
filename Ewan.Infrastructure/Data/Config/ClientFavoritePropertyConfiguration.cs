using Ewan.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ewan.Infrastructure.Data.Config
{
    public class ClientFavoritePropertyConfiguration : IEntityTypeConfiguration<ClientFavoriteProperty>
    {
        public void Configure(EntityTypeBuilder<ClientFavoriteProperty> builder)
        {
            builder.HasIndex(x => new { x.ClientId, x.PropertyId })
                .IsUnique();

            builder.HasOne(x => x.Client)
                .WithMany(x => x.FavoriteProperties)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Property)
                .WithMany(x => x.FavoritedByClients)
                .HasForeignKey(x => x.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
