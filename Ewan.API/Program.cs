
using Ewan.API.Extensions;
using Ewan.API.Helpers;
using Ewan.API.Middlewares;
using Ewan.Application;
using Ewan.Infrastructure.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Extension Methods
            builder.Services.AddIdentityService(builder.Configuration);
            builder.Services.AddApplicationService(builder.Configuration);

            var app = builder.Build();

            // Global Exception Middleware
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors("AllowAll");

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
