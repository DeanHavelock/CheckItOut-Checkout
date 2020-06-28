using CheckItOut.Payments.Application.QueryHandlers;
using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CheckItOut.Payments.UnitTests.Application.QueryHandlers
{
    public class QueryPaymentHandlerTests
    {
        [Fact]
        public async Task ReturnsPayment()
        {
            var paymentRepository = new Mock<IPaymentRepository>();

            var payment = new Payment
            {
                Amount = 1234.5m,
                CurrencyCode = "GBP",
                Status = PaymentStatus.Succeeded,
                SenderCardNumber = "4141414141414141"
            };

            paymentRepository.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(payment);

            var queryPaymentHandler = new QueryPaymentHandler(paymentRepository.Object);

            var response = await queryPaymentHandler.Query(new Domain.Queries.GetPayment { PaymentId = Guid.NewGuid().ToString() });

            Assert.Equal(payment.Amount, response.Amount);
            Assert.Equal("############4141", response.MaskedCardNumber);
        }
    }

}
