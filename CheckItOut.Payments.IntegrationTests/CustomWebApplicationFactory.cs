using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace CheckItOut.Payments.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            
            builder.ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
            {
                //var type = typeof(TStartup);
                //var path = @"C:\\OriginalApplication";

                //configurationBuilder.AddJsonFile($"{path}\\appsettings.json", optional: true, reloadOnChange: true);
                //configurationBuilder.AddEnvironmentVariables();
            });

            builder.ConfigureServices(services =>
            {

                //var serviceProvider = new ServiceCollection()
                //    .AddEntityFrameworkInMemoryDatabase()
                //    .BuildServiceProvider();
                //
                //services.AddDbContext<ApplicationDBContext>(options =>
                //{
                //    options.UseInMemoryDatabase("DBInMemoryTest");
                //    options.UseInternalServiceProvider(serviceProvider);
                //});
            });

        }


    }
}
