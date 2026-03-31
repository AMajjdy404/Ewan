using Ewan.API.Helpers;
using Ewan.Application;
using Ewan.Application.Behaviors.Ewan.Application.Behaviors;
using Ewan.Application.Features.Auth.Commands.RegisterClient.Ewan.Application.Features.Auth.Commands.RegisterClient;
using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.Mail;
using Ewan.Core.Services;
using Ewan.Infrastructure.ReposAndSpecs;
using FluentValidation;
using MediatR;
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

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(RegisterClientCommand).Assembly);
            });

            services.AddValidatorsFromAssembly(typeof(RegisterClientCommand).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
