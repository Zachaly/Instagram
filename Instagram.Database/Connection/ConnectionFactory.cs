using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Instagram.Database.Factory
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public ConnectionFactory(IConfiguration config) 
        {
            _configuration = config;
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_configuration["ConnectionStrings:SqlConnection"]);

        public IDbConnection CreateMasterConnection()
            => new SqlConnection(_configuration["ConnectionStrings:MasterConnection"]);
    }
}
