using Dapper;
using FluentMigrator.Runner;
using Instagram.Database.Factory;
using Microsoft.Extensions.Configuration;

namespace Instagram.Database.Migrations
{
    public class MigrationManager : IMigrationManager
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;
        private readonly IMigrationRunner _migrationRunner;

        public MigrationManager(IConnectionFactory connectionFactory,
            IConfiguration configuration, IMigrationRunner migrationRunner) 
        {
            _connectionFactory = connectionFactory;
            _configuration = configuration;
            _migrationRunner = migrationRunner;
        }

        public void CreateDatabase()
        {
            var query = "SELECT * FROM sys.databases WHERE name = @Name";

            using(var connection = _connectionFactory.CreateMasterConnection())
            {
                var dbName = _configuration["DatabaseName"];
                var res = connection.Query(query, new { Name = dbName });
                if(res.Any())
                {
                    return;
                }

                var createDbQuery = $"CREATE DATABASE {dbName}";
                connection.Query(createDbQuery);
            }
        }

        public void MigrateDatabase()
        {
            _migrationRunner.ListMigrations();
            _migrationRunner.MigrateUp();
        }
    }
}
