using CheckItOut.Payments.Api;
using CheckItOut.Payments.Api.Dtos;
using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.BankSim;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Domain.Queries.Projections;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CheckItOut.Payments.IntegrationTests
{
    public class PaymentTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private CustomWebApplicationFactory<Startup> _factory;
        public PaymentTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _factory.CreateClient();
        }

        [Fact]
        public async Task MakeValidPaymentCreatesPayment()
        {
            //using (var scope = _factory.Server.Host.Services.CreateScope())
            //{
                //Arrange:
                var client = _factory.CreateClient();
               
                //Act:
                var makePaymentRequest = new MakePaymentRequest
                {
                    Amount = 1000,
                    CurrencyCode = "GBP",
                    CardNumber = "4141414141414141"
                };
                var result = await client.PostAsync("/payments", new StringContent(JsonConvert.SerializeObject(makePaymentRequest), Encoding.UTF8, "application/json"));

                //Assert:
                var id = result.Headers.Location.ToString().Split('/').Last();
                var queryUrl = result.Headers.Location;
                var queryResults = await client.GetAsync(queryUrl);

                var queryContent = await queryResults.Content.ReadAsStringAsync();
                var deserializedGetPaymentResponse = JsonConvert.DeserializeObject<GetPaymentResponse>(queryContent);

                Assert.Equal(HttpStatusCode.Created, result.StatusCode);
                Assert.NotNull(result.Headers.Location);
                Assert.Equal(id, deserializedGetPaymentResponse.PaymentId.ToString());
                Assert.Equal(makePaymentRequest.Amount, deserializedGetPaymentResponse.Amount);
            //}
        }


        [Fact]
        public async Task GivenPayerWithCardDetailsAndRecipientAccountNumber_SortCode_And_FullName_WhenMakePayment_ThenExternalBankSimReturnsSuccessfulTransactionWithId()
        {
            var client = _factory.CreateClient();
            var chargeCard = new Mock<IChargeCard>();

            //Act:
            var makePaymentRequest = new MakePaymentRequest
            {
                Amount = 1000,
                CurrencyCode = "GBP",
                CardNumber = "4141414141414141"
            };
            var result = await client.PostAsync("/payments", new StringContent(JsonConvert.SerializeObject(makePaymentRequest), Encoding.UTF8, "application/json"));

        }

    }
}
