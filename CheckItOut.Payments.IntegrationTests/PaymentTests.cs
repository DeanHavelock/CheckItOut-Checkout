using CheckItOut.Payments.Api;
using CheckItOut.Payments.Api.Dtos;
using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Queries.Projections;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CheckItOut.Payments.IntegrationTests
{
    public class PaymentTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private WebApplicationFactory<Startup> _factory;
        public PaymentTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task MakeValidPaymentCreatesPayment()
        {
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
        }


    }
}
