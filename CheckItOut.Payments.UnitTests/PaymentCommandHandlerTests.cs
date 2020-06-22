using CheckItOut.Payments.Application.CommandHandlers;
using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CheckItOut.Payments.UnitTests
{
    public class PaymentCommandHandlerTests
    {
        [Fact]
        public async Task ShouldSetFieldsFromCommandToEntity()
        {
            var mockRepo = new Mock<IPaymentRepository>();
            var commandHander = new PaymentsCommandHandler(mockRepo.Object);

            var paymentCommand = new Domain.Commands.MakePaymentCommand()
            {
                Amount = 1000,
                CardNumber = "444444444444",
                MerchantId = Guid.NewGuid().ToString(),
                PaymentId = Guid.NewGuid()
            };
            await commandHander.Process(paymentCommand);
            mockRepo.Verify(x => x.Add(It.Is<Payment>(p=>Foo(p, paymentCommand))));
            Assert.True(true);
        }

        private bool Foo(Payment payment, Domain.Commands.MakePaymentCommand paymentCommand)
        {
            if(payment.Amount == paymentCommand.Amount
                && payment.CardNumber == paymentCommand.CardNumber
                && payment.Id == paymentCommand.PaymentId
                && payment.MerchantId == paymentCommand.MerchantId)
            { return true; }
            return false;
        }
    }

}
