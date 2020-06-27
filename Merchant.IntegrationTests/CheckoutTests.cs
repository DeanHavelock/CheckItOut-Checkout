using CheckItOut.Payments.Domain.Queries.Projections;
using Merchant.Application.Clients.CheckItOut.Payments.Dtos;
using Merchant.Domain.HttpContracts;
using Merchant.Ui.Web;
using Moq;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Merchant.IntegrationTests
{
    public class CheckoutTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private CustomWebApplicationFactory<Startup> _factory;
        public CheckoutTests(CustomWebApplicationFactory<Startup> factory) 
        {
            _factory = factory;
        }

        [Fact]
        public async Task MakeValidCheckoutCreatesOrder()
        {
            //Arrange:
            var client = _factory.CreateClient();

            //var mockClient = new Mock<HttpClient>();
            //mockClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(new HttpResponseMessage());

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
            var result = await client.PostAsync("/Orders", new StringContent(JsonConvert.SerializeObject(makePaymentRequest), Encoding.UTF8, "application/json"));

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
