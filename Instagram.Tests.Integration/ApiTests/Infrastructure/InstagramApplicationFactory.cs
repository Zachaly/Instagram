using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Instagram.Tests.Integration.ApiTests.Infrastructure
{
    public class InstagramApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices((services) =>
            {
                services.ConfigureRunner(c =>
                    {
                        c.WithGlobalConnectionString(Constants.ConnectionString);
                    });
                Log.Logger = new LoggerConfiguration().CreateLogger();
            });
        }
    }
}
