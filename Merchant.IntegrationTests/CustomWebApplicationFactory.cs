using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Merchant.Domain.HttpContracts;
using Merchant.Infrastructure.HttpSecureSender;

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
                var provider = services.BuildServiceProvider();
                var scope = provider.CreateScope();

            });

        }
        //public CustomWebApplicationFactory() : base()
        //{
        //        
        //}
        //protected override void ConfigureWebHost(IWebHostBuilder builder)
        //{
        //    
        //    builder.ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
        //    {
        //        //var type = typeof(TStartup);
        //        //var path = @"C:\\OriginalApplication";
        //
        //        //configurationBuilder.AddJsonFile($"{path}\\appsettings.json", optional: true, reloadOnChange: true);
        //        //configurationBuilder.AddEnvironmentVariables();
        //    });
        //
        //    builder.ConfigureServices(services =>
        //    {
        //        services.AddTransient<IPostToSecureHttpEndpointWithRetries, PostToSecureHttpEndpointWithRetries>();
        //        var provider = services.BuildServiceProvider();
        //        var scope = provider.CreateScope();
        //        //var merchantRepository = scope.ServiceProvider.GetRequiredService<IMerchantRepository>();
        //        //
        //        //var merchant = merchantRepository.GetById("Test").Result;
        //        //if (merchant == null)
        //        //{
        //        //    SetupInitialTestData(merchantRepository);
        //        //}
        //        //var serviceProvider = new ServiceCollection()
        //        //    .AddEntityFrameworkInMemoryDatabase()
        //        //    .BuildServiceProvider();
        //        //
        //        //services.AddDbContext<ApplicationDBContext>(options =>
        //        //{
        //        //    options.UseInMemoryDatabase("DBInMemoryTest");
        //        //    options.UseInternalServiceProvider(serviceProvider);
        //        //});
        //    });
        //
        //}

    }
}
