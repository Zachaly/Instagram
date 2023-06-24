using Instagram.Api.Authorization;
using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Migrations;
using Instagram.Database.Repository;
using Instagram.Domain.Enum;
using Instagram.Models.UserClaim.Request;
using MediatR;

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

        public static async Task CreateAdmin(this WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();
                var userRoleService = scope.ServiceProvider.GetRequiredService<IUserClaimService>();

                var adminCount = await userRoleService.GetCountAsync(new GetUserClaimRequest { Value = UserClaimValues.Admin });

                if(adminCount < 1)
                {
                    logger.LogInformation("Admin creation");
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    var request = new RegisterCommand
                    {
                        Email = app.Configuration["DefaultAdmin:Email"],
                        Gender = Gender.NotSpecified,
                        Name = "admin",
                        Nickname = app.Configuration["DefaultAdmin:Nickname"],
                        Password = app.Configuration["DefaultAdmin:Password"]
                    };

                    await mediator.Send(request);

                    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                    var user = await userRepository.GetEntityByEmailAsync(request.Email);

                    var addClaimRequest = new AddUserClaimRequest
                    {
                        UserId = user.Id,
                        Value = UserClaimValues.Admin
                    };

                    var claimService = scope.ServiceProvider.GetRequiredService<IUserClaimService>();

                    await claimService.AddAsync(addClaimRequest);

                    logger.LogInformation("Admin created");
                }
            }
            
        }
    }
}
