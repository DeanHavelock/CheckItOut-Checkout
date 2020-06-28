using CheckItOut.Payments.Domain.Commands;
using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Interfaces;
using System.Threading.Tasks;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Domain.BankSim;
using CheckItOut.Payments.Domain.BankSim.Dto;
using CheckItOut.Payments.Domain.Queries;
using System;

namespace CheckItOut.Payments.Application.CommandHandlers
{
    public class PaymentsCommandHandler : IPaymentsCommandHandler
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IChargeCard _chargeCard;
        private readonly IQueryMerchants _merchantQueries;

        public PaymentsCommandHandler(IPaymentRepository paymentRepository, IQueryMerchants merchantQueries, IChargeCard chargeCard)
        {
            _paymentRepository = paymentRepository;
            _chargeCard = chargeCard;
            _merchantQueries = merchantQueries;
        }

        public async Task Handle(MakePaymentCommand command)
        {
            var duplicatePaymentAttempt = await _paymentRepository.GetByInvoiceId(command.InvoiceId);
             if (duplicatePaymentAttempt != null && !string.IsNullOrWhiteSpace(duplicatePaymentAttempt.InvoiceId)) 
                throw new Exception("Duplicate Payment Attempt, PaymentId with InvoiceId: " + command.InvoiceId + " already exists");

            var recipient = await _merchantQueries.GetById(command.RecipientMerchantId);
            var chargeRequest = new FinaliseTransactionRequest
            {
                InvoiceId = command.InvoiceId,
                RecipientAccountNumber = recipient.AccountNumber,
                RecipientSortCode = recipient.SortCode,
                SenderCardNumber = command.SenderCardNumber,
                SenderCvv = command.SenderCvv,
                SenderCardExpiryMonth = command.SenderCardExpiryMonth,
                SenderCardExpiryYear = command.SenderCardExpiryYear,
                Amount = command.Amount,
                CurrencyCode = command.CurrencyCode,
            };
            
            //Save
            var payment = new Payment
            {
                PaymentId = command.PaymentId,
                OrderId = command.OrderId,
                InvoiceId = command.InvoiceId,
                RecipientMerchantId = recipient.MerchantId,
                SenderCardNumber = command.SenderCardNumber,
                Amount = command.Amount,
                CurrencyCode = command.CurrencyCode,
            };

            await _paymentRepository.Add(payment);
            await _paymentRepository.Save();
            
            //ToDo: Implement Retries and Auth to Endpoint
            //MakePayment
            var chargeResponse = await _chargeCard.Charge(chargeRequest);

            if (chargeResponse.Success)
                payment.Succeed(chargeResponse.BankSimTransactionId);
            else
                payment.Fail("return a reason from bank");
           
            //await _paymentRepository.Add(payment);
            await _paymentRepository.Save();
        }
    }
}
