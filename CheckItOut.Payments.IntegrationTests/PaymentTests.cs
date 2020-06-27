using CheckItOut.Payments.Api;
using CheckItOut.Payments.Api.Dtos;
using CheckItOut.Payments.Domain.BankSim;
using CheckItOut.Payments.Domain.Queries.Projections;
using Moq;
using Newtonsoft.Json;
using System;
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
            //Arrange:
            var client = _factory.CreateClient();

            //Act:
            var makePaymentRequest = new MakeGuestToMerchantPaymentRequest
            {
                Amount = 1000,
                CurrencyCode = "GBP",
                SenderCardNumber = "4141414141414141",
                SenderCvv = "111",
                RecipientMerchantId = "TEST",
                InvoiceId = Guid.NewGuid().ToString()
            };

            var result = await client.PostAsync("/payments", new StringContent(JsonConvert.SerializeObject(makePaymentRequest), Encoding.UTF8, "application/json"));

            //Assert:
            var paymentId = result.Headers.Location.ToString().Split('/').Last();
            var queryUrl = result.Headers.Location;
            var queryResults = await client.GetAsync(queryUrl);

            var queryContent = await queryResults.Content.ReadAsStringAsync();
            var deserializedGetPaymentResponse = JsonConvert.DeserializeObject<GetPaymentResponse>(queryContent);

            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.NotNull(result.Headers.Location);
            Assert.Equal(paymentId, deserializedGetPaymentResponse.PaymentId.ToString());
            Assert.Equal(makePaymentRequest.Amount, deserializedGetPaymentResponse.Amount);
        }


    }
}
