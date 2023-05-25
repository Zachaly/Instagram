using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Instagram.Tests.Integration.ApiTests
{
    public class InstagramApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            
            builder.ConfigureServices((IServiceCollection services) =>
            {
                services.ConfigureRunner(c =>
                    {
                        c.WithGlobalConnectionString(Constants.ConnectionString);
                    });
            });
        }
    }
}
