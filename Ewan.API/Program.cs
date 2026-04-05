
using Ewan.API.Extensions;
using Ewan.API.Middlewares;
using Ewan.Application;
using Ewan.Application.Helpers;
using Ewan.Infrastructure.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Ewan.API
{

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "EWAN API",
                    Version = "v1",
                    Description = "This is API for Ewan Hotel Management System",
                    Contact = new OpenApiContact
                    {
                        Name = "Abdalla Magdy",
                        Email = "abdullamajdy493@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/abdalla-magdy-5437a4293")
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Token like this: Bearer {your token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                 {
                   new OpenApiSecurityScheme
                   {
                       Reference = new OpenApiReference
                       {
                           Type = ReferenceType.SecurityScheme,
                           Id = "Bearer"
                       }
                   },
                      new string[] {}
                   }
                 });
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Extension Methods
            builder.Services.AddIdentityService(builder.Configuration);
            builder.Services.AddApplicationService(builder.Configuration);
            builder.Services.AddCustomCors(builder.Configuration);

            var app = builder.Build();

            // Global Exception Middleware
            app.UseMiddleware<ExceptionMiddleware>();


            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard();

            app.MapControllers();

            app.Lifetime.ApplicationStarted.Register(() =>
            {
                using var scope = app.Services.CreateScope();
                var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

                recurringJobManager.AddOrUpdate<RefreshTokenCleanupJob>(
                    "refresh-token-cleanup",
                    job => job.ExecuteAsync(),
                    builder.Configuration.GetValue<string>("RefreshTokenCleanup:CronExpression") ?? Cron.Daily());
            });

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var dbContext = services.GetRequiredService<AppDbContext>();
                var seeder = services.GetRequiredService<DataSeeder>();

                await dbContext.Database.MigrateAsync();
                await seeder.SeedAsync();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred while applying migrations or seeding data.");
            }

            app.Run();
        }
    }
}
