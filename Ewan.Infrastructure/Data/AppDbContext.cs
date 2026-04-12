using Ewan.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ewan.Infrastructure.Data
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ClientPasswordResetToken> ClientPasswordResetTokens { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<PropertyFacility> PropertyFacilities { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<ClientFavoriteProperty> ClientFavoriteProperties { get; set; }
        public DbSet<PropertyRating> PropertyRatings { get; set; }
        public DbSet<ContactUsSetting> ContactUsSettings { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<TermsAndConditionsSetting> TermsAndConditionsSettings { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
