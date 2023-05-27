using Instagram.Database.Migrations;

namespace Instagram.Api.Infrastructure
{
    public static class WebApplicationExtensions
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                var migrationManager = scope.ServiceProvider.GetRequiredService<IMigrationManager>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<MigrationManager>>();

                try
                {
                    migrationManager.CreateDatabase();
                    logger.LogInformation("Database created");
                }
                catch 
                {
                    logger.LogCritical("Failed to create database");
                    return app;
                }

                try
                {
                    migrationManager.MigrateDatabase();
                    logger.LogInformation("Database migrated");
                }
                catch
                {
                    logger.LogCritical("Failed to migrate database");
                }
                
                return app;
            }
        }
    }
}
