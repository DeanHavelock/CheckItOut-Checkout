using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using Xunit;

namespace CheckItOut.Payments.IntegrationTests
{
    public class Payment
    {
        [Fact]
        public void MakeValidPaymentCreatesPayment()
        {
            //Arrange:
            HttpClient httpClient = new HttpClient();

            //Act:
            var response = httpClient.GetAsync("https://localhost:44379/payments").Result;

            //Assert:
            Assert.True(false);
        }
    }
}
