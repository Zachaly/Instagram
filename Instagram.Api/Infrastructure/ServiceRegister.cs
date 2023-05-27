using FluentMigrator.Runner;
using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Factory;
using Instagram.Database.Migrations;
using Instagram.Database.Repository;
using Instagram.Database.Sql;

namespace Instagram.Api.Infrastructure
{
    public static class ServiceRegister
    {
        public static void RegisterDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IMigrationManager, MigrationManager>();
            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<ISqlQueryBuilder, SqlQueryBuilder>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddFluentMigratorCore()
                .ConfigureRunner(c =>
                {
                    c.AddSqlServer2016();
                    c.WithGlobalConnectionString(config["ConnectionStrings:SqlConnection"]);
                    c.ScanIn(typeof(MigrationManager).Assembly).For.Migrations();
                });
        }

        public static void RegisterApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            
            services.AddScoped<IUserFactory, UserFactory>();
            services.AddScoped<IResponseFactory, ResponseFactory>();

            services.AddMediatR(opt =>
            {
                opt.RegisterServicesFromAssemblyContaining<LoginCommand>();
            });
        }
    }
}
