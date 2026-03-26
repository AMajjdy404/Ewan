using Ewan.API.Helpers;
using Ewan.Application;
using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.Mail;
using Ewan.Core.Services;
using Ewan.Infrastructure.ReposAndSpecs;
using Microsoft.AspNetCore.Identity;

namespace Ewan.API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<DataSeeder>();
            services.AddScoped<IPasswordHasher<Client>, PasswordHasher<Client>>();

            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            return services;
        }
    }
}
