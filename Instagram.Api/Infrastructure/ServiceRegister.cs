using FluentMigrator.Runner;
using Instagram.Database.Factory;
using Instagram.Database.Migration;

namespace Instagram.Api.Infrastructure
{
    public static class ServiceRegister
    {
        public static void RegisterDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IMigrationManager, MigrationManager>();
            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            services.AddFluentMigratorCore()
                .ConfigureRunner(c =>
                {
                    c.AddSqlServer2016();
                    c.WithGlobalConnectionString(config.GetConnectionString("DatabaseConnection"));
                    c.ScanIn(typeof(MigrationManager).Assembly).For.Migrations();
                });
        }
    }
}
