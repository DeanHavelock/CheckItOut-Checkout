using CheckItOut.Payments.Domain.Commands;
using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Interfaces;
using System.Threading.Tasks;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Domain.BankSim;
using CheckItOut.Payments.Domain.BankSim.Dto;
using CheckItOut.Payments.Domain.Queries;
using System;
using CheckItOut.Payments.Domain.MerchantContracts;

namespace CheckItOut.Payments.Application.CommandHandlers
{
    public class PaymentsCommandHandler : IPaymentsCommandHandler
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IChargeCard _chargeCard;
        private readonly IQueryMerchants _merchantQueries;
        //private readonly INotifyMerchantPaymentSubmitted _notifyMerchantPaymentSubmitted;
        private readonly INotifyMerchantPaymentSucceeded _notifyMerchantPaymentSucceeded;

        public PaymentsCommandHandler(IPaymentRepository paymentRepository, IQueryMerchants merchantQueries, IChargeCard chargeCard, /*INotifyMerchantPaymentSubmitted _notifyMerchantPaymentSubmitted, */INotifyMerchantPaymentSucceeded notifyMerchantPaymentSucceeded)
        {
            _paymentRepository = paymentRepository;
            _chargeCard = chargeCard;
            _merchantQueries = merchantQueries;
            _notifyMerchantPaymentSucceeded = notifyMerchantPaymentSucceeded;
        }

        public async Task Handle(MakePaymentFromInternalCommand command)
        {
            //idempotent check
            var duplicatePaymentAttempt = await _paymentRepository.GetByInvoiceId(command.InvoiceId);
            if (duplicatePaymentAttempt != null && !string.IsNullOrWhiteSpace(duplicatePaymentAttempt.InvoiceId))
                throw new Exception("Duplicate Payment Attempt, PaymentId with InvoiceId: " + command.InvoiceId + " already exists");

            //submit notification to Merchant (to create order)
            //await _notifyMerchantPaymentSubmitted.Notify();

            //Prepaire ChargeRequest with Sender And Merchant Details
            var recipient = await _merchantQueries.GetById(command.RecipientMerchantId);
            var chargeRequest = MapToChargeRequest(command, recipient);

            //Save Payment Request
            var payment = MapToPayment(command, recipient);
            await _paymentRepository.Add(payment);
            await _paymentRepository.Save();

            //ToDo: Implement Retries and Auth to Endpoint
            //MakePayment
            var chargeResponse = await _chargeCard.Charge(chargeRequest);
            if (chargeResponse.Success)
            {
                payment.Succeed(chargeResponse.BankSimTransactionId);
                //notify Merchant of PaymentSuccessful (to update Order.PaymentStatus)
                await _notifyMerchantPaymentSucceeded.Notify();
            }
            else
                payment.Fail("return a reason from bank");

            await _paymentRepository.Save();
        }

        public async Task Handle(MakePaymentCommand command)
        {
            var duplicatePaymentAttempt = await _paymentRepository.GetByInvoiceId(command.InvoiceId);
             if (duplicatePaymentAttempt != null && !string.IsNullOrWhiteSpace(duplicatePaymentAttempt.InvoiceId)) 
                throw new Exception("Duplicate Payment Attempt, PaymentId with InvoiceId: " + command.InvoiceId + " already exists");

            //Prepaire ChargeRequest with Sender And Merchant Details
            var recipient = await _merchantQueries.GetById(command.RecipientMerchantId);
            var chargeRequest = MapToChargeRequest(command, recipient);

            //Save Payment Request
            var payment = MapToPayment(command, recipient);
            await _paymentRepository.Add(payment);
            await _paymentRepository.Save();

            //ToDo: Implement Retries and Auth to Endpoint
            //MakePayment
            var chargeResponse = await _chargeCard.Charge(chargeRequest);
            if (chargeResponse.Success)
            {
                payment.Succeed(chargeResponse.BankSimTransactionId);
            }
                
            else
                payment.Fail("return a reason from bank");
           
            await _paymentRepository.Save();
        }

        private Payment MapToPayment(MakePaymentCommand command, Merchant recipient)
        {
            return new Payment
            {
                PaymentId = command.PaymentId,
                OrderId = command.OrderId,
                InvoiceId = command.InvoiceId,
                RecipientMerchantId = recipient.MerchantId,
                SenderCardNumber = command.SenderCardNumber,
                Amount = command.Amount,
                CurrencyCode = command.CurrencyCode,
            };
        }

        private FinaliseTransactionRequest MapToChargeRequest(MakePaymentCommand command, Merchant recipient)
        {
            return new FinaliseTransactionRequest
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
        }
    }
}
