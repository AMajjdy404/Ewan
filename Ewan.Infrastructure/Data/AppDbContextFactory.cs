using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Text.Json;

namespace Ewan.Infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            var appSettingsPath = FindAppSettingsPath(basePath);
            if (appSettingsPath == null)
                throw new InvalidOperationException("Could not find appsettings.json for design-time DbContext creation.");

            var connectionString = ReadDefaultConnectionString(appSettingsPath);
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing.");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }

        private static string? FindAppSettingsPath(string startPath)
        {
            var dir = new DirectoryInfo(startPath);

            while (dir != null)
            {
                var direct = Path.Combine(dir.FullName, "appsettings.json");
                if (File.Exists(direct))
                    return direct;

                var apiPath = Path.Combine(dir.FullName, "Ewan.API", "appsettings.json");
                if (File.Exists(apiPath))
                    return apiPath;

                dir = dir.Parent;
            }

            return null;
        }

        private static string? ReadDefaultConnectionString(string appSettingsPath)
        {
            var json = File.ReadAllText(appSettingsPath);
            using var document = JsonDocument.Parse(json);

            if (!document.RootElement.TryGetProperty("ConnectionStrings", out var connectionStrings))
                return null;

            if (!connectionStrings.TryGetProperty("DefaultConnection", out var defaultConnection))
                return null;

            return defaultConnection.GetString();
        }
    }
}
