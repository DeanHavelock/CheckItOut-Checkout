using CheckItOut.Payments.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
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
            var result = await client.PostAsync("/payments", new StringContent(JsonConvert.SerializeObject(new { Amount = 10 }), Encoding.UTF8, "application/json"));
            Assert:
            Assert.True(false);
        }


    }
}
