using CheckItOut.Payments.Application.CommandHandlers;
using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.BankSim;
using CheckItOut.Payments.Domain.BankSim.Dto;
using CheckItOut.Payments.Domain.Commands;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Domain.Queries;
using Moq;
using System;
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
            var mockMercharQuery = new Mock<IQueryMerchants>();
            var mockBankSim = new Mock<IChargeCard>();
            var commandHander = new PaymentsCommandHandler(mockRepo.Object, mockMercharQuery.Object, mockBankSim.Object);

            var paymentCommand = new Domain.Commands.MakePaymentCommand()
            {
                PaymentId = Guid.NewGuid().ToString(),
                InvoiceId = Guid.NewGuid().ToString(),
                RecipientMerchantId = Guid.NewGuid().ToString(),
                SenderCardNumber = "444444444444",
                SenderCvv = "111",
                Amount = 1000
            };
            await commandHander.Handle(paymentCommand);
            mockRepo.Verify(x => x.Add(It.Is<Payment>(p=> CheckCommandMappedToPayment(p, paymentCommand))));
            Assert.True(true);
        }

        [Fact]
        public async Task MakePaymentCallsChargeCardOnBankApi()
        {
            var bankSimChargeCard = new Mock<IChargeCard>();
            var mockRepo = new Mock<IPaymentRepository>();
            var merchantQueries = new Mock<IQueryMerchants>();

            var merchant = new Merchant { AccountNumber = "888888" };

            merchantQueries.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(merchant);
                
            var commandHander = new PaymentsCommandHandler(mockRepo.Object, merchantQueries.Object, bankSimChargeCard.Object);

            var command = new MakePaymentCommand { InvoiceId = Guid.NewGuid().ToString(), RecipientMerchantId = Guid.NewGuid().ToString(), SenderCardNumber = "4444444444444444", SenderCvv="111", PaymentId = Guid.NewGuid().ToString(), Amount = 100 };                 

            await commandHander.Handle(command);

            bankSimChargeCard.Verify(x => x.Charge(It.Is<FinaliseTransactionRequest>(request =>
                request.RecipientAccountNumber == merchant.AccountNumber
                && request.SenderCardNumber == command.SenderCardNumber)));
       
        }

        [Fact]
        public async Task ShouldMapBankTransactionDetailsOnPayment()
        {
            var mockBankSimChargeCard = new Mock<IChargeCard>();
            var mockRepo = new Mock<IPaymentRepository>();
            var mockMerchantQueries = new Mock<IQueryMerchants>();

            var merchant = new Merchant { AccountNumber = "888888" };
            var chargeResponse = new FinaliseTransactionResponse { BankSimTransactionId = Guid.NewGuid().ToString() };

            mockMerchantQueries.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(merchant);
            mockBankSimChargeCard.Setup(x => x.Charge(It.IsAny<FinaliseTransactionRequest>())).ReturnsAsync(chargeResponse);

            var commandHander = new PaymentsCommandHandler(mockRepo.Object, mockMerchantQueries.Object, mockBankSimChargeCard.Object);

            var command = new MakePaymentCommand { InvoiceId = Guid.NewGuid().ToString(), RecipientMerchantId = Guid.NewGuid().ToString(), SenderCardNumber = "4444444444444444", SenderCvv = "111", PaymentId = Guid.NewGuid().ToString(), Amount = 100 };

            await commandHander.Handle(command);

            mockRepo.Verify(x => x.Add(It.Is<Payment>(payment => payment.BankSimTransactionId == chargeResponse.BankSimTransactionId)));
        }

        

        private bool CheckCommandMappedToPayment(Payment payment, Domain.Commands.MakePaymentCommand paymentCommand)
        {
            if(payment.Amount == paymentCommand.Amount
                && payment.SenderCardNumber == paymentCommand.SenderCardNumber
                && payment.PaymentId == paymentCommand.PaymentId
                && payment.RecipientMerchantId == paymentCommand.RecipientMerchantId)
            { return true; }
            return false;
        }
    }

}
