using CheckItOut.Payments.Application.CommandHandlers;
using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.BankSim;
using CheckItOut.Payments.Domain.BankSim.Dto;
using CheckItOut.Payments.Domain.Commands;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Domain.MerchantContracts;
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
        public async Task ShouldSetFieldsAndMaskedCardNumberFromCommandToEntity()
        {
            var mockRepo = new Mock<IPaymentRepository>();
            var mockMercharQuery = new Mock<IQueryMerchants>();
            var mockBankSim = new Mock<IChargeCardAdapter>();
            var mockNotifyMerchantPaymentSucceeded = new Mock<INotifyMerchantPaymentSucceeded>();

            var commandHander = new PaymentsCommandHandler(mockRepo.Object, mockMercharQuery.Object, mockBankSim.Object, mockNotifyMerchantPaymentSucceeded.Object);
            string maskedCardNumber = "############4141";
            var paymentCommand = new Domain.Commands.MakePaymentCommand()
            {
                InvoiceId = Guid.NewGuid().ToString(),
                RecipientMerchantId = Guid.NewGuid().ToString(),
                SenderCardNumber = "4141414141414141",
                SenderCvv = "111",
                Amount = 1000
            };

            mockMercharQuery.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(new Merchant { MerchantId=paymentCommand.RecipientMerchantId });
            mockBankSim.Setup(x => x.Charge(It.IsAny<FinaliseTransactionRequest>())).ReturnsAsync(new FinaliseTransactionResponse { Success = true, BankSimTransactionId = Guid.NewGuid().ToString() });

            await commandHander.Handle(paymentCommand);
            mockRepo.Verify(x => x.Add(It.Is<Payment>(p => p.Amount == paymentCommand.Amount && p.InvoiceId == paymentCommand.InvoiceId && p.RecipientMerchantId == paymentCommand.RecipientMerchantId && p.SenderCardNumber == maskedCardNumber)));
        }

        [Fact]
        public async Task MakePaymentCallsChargeCardOnBankApi()
        {
            var bankSimChargeCard = new Mock<IChargeCardAdapter>();
            var mockRepo = new Mock<IPaymentRepository>();
            var merchantQueries = new Mock<IQueryMerchants>();
            var mockNotifyMerchantPaymentSucceeded = new Mock<INotifyMerchantPaymentSucceeded>();

            var merchant = new Merchant { AccountNumber = "888888" };

            merchantQueries.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(merchant);

            var commandHander = new PaymentsCommandHandler(mockRepo.Object, merchantQueries.Object, bankSimChargeCard.Object, mockNotifyMerchantPaymentSucceeded.Object);

            var command = new MakePaymentCommand { InvoiceId = Guid.NewGuid().ToString(), RecipientMerchantId = Guid.NewGuid().ToString(), SenderCardNumber = "4444444444444444", SenderCvv = "111", PaymentId = Guid.NewGuid().ToString(), Amount = 100 };

            bankSimChargeCard.Setup(x => x.Charge(It.IsAny<FinaliseTransactionRequest>())).ReturnsAsync(new FinaliseTransactionResponse());

            await commandHander.Handle(command);

            bankSimChargeCard.Verify(x => x.Charge(It.Is<FinaliseTransactionRequest>(request =>
                request.RecipientAccountNumber == merchant.AccountNumber
                && request.SenderCardNumber == command.SenderCardNumber)));
        }

        [Fact]
        public async Task ShouldMapBankTransactionDetailsOnPayment()
        {
            var mockBankSimChargeCard = new Mock<IChargeCardAdapter>();
            var mockRepo = new Mock<IPaymentRepository>();
            var mockMerchantQueries = new Mock<IQueryMerchants>();
            var mockNotifyMerchantPaymentSucceeded = new Mock<INotifyMerchantPaymentSucceeded>();

            var merchant = new Merchant { AccountNumber = "888888" };
            var chargeResponse = new FinaliseTransactionResponse { BankSimTransactionId = Guid.NewGuid().ToString(), Success = true };

            mockMerchantQueries.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(merchant);
            mockBankSimChargeCard.Setup(x => x.Charge(It.IsAny<FinaliseTransactionRequest>())).ReturnsAsync(chargeResponse);

            var commandHander = new PaymentsCommandHandler(mockRepo.Object, mockMerchantQueries.Object, mockBankSimChargeCard.Object, mockNotifyMerchantPaymentSucceeded.Object);

            var command = new MakePaymentCommand { InvoiceId = Guid.NewGuid().ToString(), RecipientMerchantId = Guid.NewGuid().ToString(), SenderCardNumber = "4444444444444444", SenderCvv = "111", PaymentId = Guid.NewGuid().ToString(), Amount = 100 };

            mockBankSimChargeCard.Setup(x => x.Charge(It.IsAny<FinaliseTransactionRequest>())).ReturnsAsync(chargeResponse);

            await commandHander.Handle(command);

            mockRepo.Verify(x => x.Add(It.Is<Payment>(payment => payment.BankSimTransactionId == chargeResponse.BankSimTransactionId)));
        }
    }


}
