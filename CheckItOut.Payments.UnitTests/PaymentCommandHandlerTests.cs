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
                Amount = 1000,
                CardNumber = "444444444444",
                MerchantId = Guid.NewGuid().ToString(),
                PaymentId = Guid.NewGuid()
            };
            await commandHander.Process(paymentCommand);
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

            var command = new MakePaymentCommand { Amount = 100, CardNumber = "4444444444444444", MerchantId = Guid.NewGuid().ToString(), PaymentId = Guid.NewGuid() };                 

            await commandHander.Process(command);

            bankSimChargeCard.Verify(x => x.Charge(It.Is<FinaliseTransactionRequest>(request =>
                request.RecipientAccountNumber == merchant.AccountNumber
                && request.SenderCardNumber == command.CardNumber)));
       
        }

        [Fact]
        public async Task ShouldMapBankTransactionDetailsOnPayment()
        {
            var mockBankSimChargeCard = new Mock<IChargeCard>();
            var mockRepo = new Mock<IPaymentRepository>();
            var mockMerchantQueries = new Mock<IQueryMerchants>();

            var merchant = new Merchant { AccountNumber = "888888" };
            var chargeResponse = new FinaliseTransactionResponse { TransactionId = Guid.NewGuid().ToString() };

            mockMerchantQueries.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(merchant);
            mockBankSimChargeCard.Setup(x => x.Charge(It.IsAny<FinaliseTransactionRequest>())).ReturnsAsync(chargeResponse);

            var commandHander = new PaymentsCommandHandler(mockRepo.Object, mockMerchantQueries.Object, mockBankSimChargeCard.Object);

            var command = new MakePaymentCommand { Amount = 100, CardNumber = "4444444444444444", MerchantId = Guid.NewGuid().ToString(), PaymentId = Guid.NewGuid() };

            await commandHander.Process(command);

            mockRepo.Verify(x => x.Add(It.Is<Payment>(payment => payment.TransactionId == chargeResponse.TransactionId)));
        }

        

        private bool CheckCommandMappedToPayment(Payment payment, Domain.Commands.MakePaymentCommand paymentCommand)
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
