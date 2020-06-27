using Merchant.Ui.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Merchant.Domain.HttpContracts;
using System.Net.Http;
using Merchant.Application.Clients.CheckItOut.Payments.Dtos;
using Newtonsoft.Json;
using System.Net;
using System.Linq;
using CheckItOut.Payments.Domain.Queries.Projections;

namespace Merchant.IntegrationTests
{
    public class CheckoutTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private CustomWebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        public CheckoutTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddTransient<IPostToSecureHttpEndpointWithRetries, MockPostToSecureHttpEndpointWithRetries>();
                    //var privateServiceProvider = services.BuildServiceProvider();

                    //using (var scope = _privateServiceProvider.CreateScope())
                    //{
                    //    var scopedServices = scope.ServiceProvider;
                    //    //var db = scopedServices
                    //    //    .GetRequiredService<ApplicationDbContext>();
                    //    //var logger = scopedServices
                    //    //    .GetRequiredService<ILogger<IndexPageTests>>();
                    //
                    //    //try
                    //    //{
                    //    //    Utilities.ReinitializeDbForTests(db);
                    //    //}
                    //    //catch (Exception ex)
                    //    //{
                    //    //    logger.LogError(ex, "An error occurred seeding " +
                    //    //        "the database with test messages. Error: {Message}",
                    //    //        ex.Message);
                    //    //}
                    //}
                });
            })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
        }

        [Fact]
        public async Task TestInitialisationOfDependencyOverrideFromInsideXunitIntegrationTestUsingMockService()
        {
            var makePaymentRequest = new MakeGuestToMerchantPaymentRequest
            {
                Amount = 1000,
                CurrencyCode = "GBP",
                SenderCardNumber = "4141414141414141",
                SenderCvv = "111",
                RecipientMerchantId = "TEST",
                InvoiceId = Guid.NewGuid().ToString()
            };

            //Act:
            var result = await _client.PostAsync("/Orders", new StringContent(JsonConvert.SerializeObject(makePaymentRequest), Encoding.UTF8, "application/json"));

            //Assert:
            var paymentId = result.Headers.Location.ToString().Split('/').Last();
            var queryUrl = result.Headers.Location;
            var queryResults = await _client.GetAsync(queryUrl);

            var queryContent = await queryResults.Content.ReadAsStringAsync();
            var deserializedGetPaymentResponse = JsonConvert.DeserializeObject<GetPaymentResponse>(queryContent);

            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.NotNull(result.Headers.Location);
            Assert.Equal(paymentId, deserializedGetPaymentResponse.PaymentId.ToString());
            Assert.Equal(makePaymentRequest.Amount, deserializedGetPaymentResponse.Amount);
        }
    }
    
}
