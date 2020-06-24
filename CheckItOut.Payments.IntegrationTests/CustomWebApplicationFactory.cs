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
                var provider = services.BuildServiceProvider();
                var scope = provider.CreateScope();
                var merchantRepository = scope.ServiceProvider.GetRequiredService<IMerchantRepository>();

                var merchant = merchantRepository.GetById("Test").Result;
                if (merchant == null)
                {
                    SetupInitialTestData(merchantRepository);
                }
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

        private void SetupInitialTestData(IMerchantRepository merchantRepository)
        {
            var newMerchant = new Merchant() { Id = "TEST", FullName = "bob", AccountNumber = "1111111111111111", SortCode = "111111" };
            merchantRepository.Add(newMerchant).Wait();
            merchantRepository.Save().Wait();
        }
    }
}
