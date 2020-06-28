using CheckItOut.Payments.Api;
using CheckItOut.Payments.Api.Dtos;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Domain.Queries.Projections;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using CheckItOut.Payments.Domain;
using Moq;
using CheckItOut.Payments.Domain.BankSim;
using CheckItOut.Payments.Domain.BankSim.Dto;

namespace CheckItOut.Payments.IntegrationTests
{
    public class PaymentTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private CustomWebApplicationFactory<Startup> _factory;
        private HttpClient _client;
        private Mock<IChargeCard> _chargeCard;

        private void SetupInitialTestData(IMerchantRepository merchantRepository)
        {
            var newMerchant = new Merchant() { MerchantId = "TEST", FullName = "bob", AccountNumber = "1111111111111111", SortCode = "111111" };
            merchantRepository.Add(newMerchant).Wait();
            merchantRepository.Save().Wait();
        }

        public PaymentTests(CustomWebApplicationFactory<Startup> factory)
        {
            _chargeCard = new Mock<IChargeCard>();

            _factory = factory;
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IChargeCard>(_chargeCard.Object);
                    var privateServiceProvider = services.BuildServiceProvider();

                    using (var scope = privateServiceProvider.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var merchantRepository = scopedServices.GetRequiredService<IMerchantRepository>();
                        var merchant = merchantRepository.GetById("Test").Result;
                        if (merchant == null)
                        {
                            SetupInitialTestData(merchantRepository);
                        }
                        //var db = scopedServices
                        //    .GetRequiredService<ApplicationDbContext>();
                        //var logger = scopedServices
                        //    .GetRequiredService<ILogger<IndexPageTests>>();
                    }
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task MakeValidPaymentCreatesPayment()
        {
            //Arrange:
            _chargeCard.Setup(x => x.Charge(It.IsAny<FinaliseTransactionRequest>())).ReturnsAsync(new FinaliseTransactionResponse { BankSimTransactionId = "1234" });

            
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
            var result = await _client.PostAsync("/payments", new StringContent(JsonConvert.SerializeObject(makePaymentRequest), Encoding.UTF8, "application/json"));

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

        [Fact]
        public async Task FailedPaymentRequest() 
        { 

        }
            
     }
}
