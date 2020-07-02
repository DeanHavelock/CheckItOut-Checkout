using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Merchant.IntegrationTests
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
                //services.AddTransient<IPostToSecureHttpEndpointWithRetries, MockPostToSecureHttpEndpointWithRetries>();
                var provider = services.BuildServiceProvider();
                var scope = provider.CreateScope();
            });
        
        }

    }
}
