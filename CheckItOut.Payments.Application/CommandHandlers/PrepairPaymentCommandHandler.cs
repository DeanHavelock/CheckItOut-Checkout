using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Commands;
using CheckItOut.Payments.Domain.Interfaces;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Domain.Queries;
using System;

namespace CheckItOut.Payments.Application.CommandHandlers
{
    public partial class PrepairPaymentCommandHandler : IPrepairPaymentCommandHandler
    {
        private readonly IPaymentRequestRepository _paymentRequestRepository;
        private readonly IQueryMerchants _merchantQueries;

        public PrepairPaymentCommandHandler(IPaymentRequestRepository paymentRequestRepository, IQueryMerchants merchantQueries)
        {
            _paymentRequestRepository = paymentRequestRepository;       
            _merchantQueries = merchantQueries;
        }

        public string Handle(PrepairGuestPaymentRequest command)
        {
            var duplicatePaymentAttempt = _paymentRequestRepository.GetByInvoiceId(command.InvoiceId).Result;
            if (duplicatePaymentAttempt != null && !string.IsNullOrWhiteSpace(duplicatePaymentAttempt.InvoiceId))
                throw new Exception("Duplicate PaymentRequest Attempt, PaymentId with InvoiceId: " + command.InvoiceId + " already exists");

            //Prepaire PaymentRequest with InvoiceId, Amount And Merchant Details
            var recipient = _merchantQueries.GetById(command.RecipientMerchantId).Result;
            var paymentRequest = MapToPaymentRequest(command, recipient);

            //Save PaymentRequest
            _paymentRequestRepository.Add(paymentRequest).Wait();
            _paymentRequestRepository.Save().Wait();

            ///ToDo: Implement Retries and Auth to Endpoint
            ///MakePayment
            //var chargeResponse = await _chargeCard.Charge(chargeRequest);
            //if (chargeResponse.Success)
            //{
            //    payment.Succeed(chargeResponse.BankSimTransactionId);
            //    _paymentRepository.Save().Wait();
            //    //await _notifyMerchantPaymentSucceeded.Notify(command.InvoiceId, payment.PaymentId, recipient.MerchantId);
            //    //await _notifyCustomerByEmailPaymentSucceeded.Notify(command.SenderEmail, command.InvoiceId, payment.PaymentId, recipient.MerchantId);
            //}
            //else
            //{
            //    payment.Fail("return a reason from bank");
            //    _paymentRepository.Save().Wait();
            //}
            return paymentRequest.PaymentRequestId;
        }

        private PaymentRequest MapToPaymentRequest(PrepairGuestPaymentRequest command, Merchant recipient)
        {
            return new PaymentRequest
            {
                PaymentRequestId = Guid.NewGuid().ToString(),
                InvoiceId = command.InvoiceId,
                MerchantId = recipient.MerchantId,
                Amount = command.Amount,
                CurrencyCode = command.CurrencyCode,
            };
        }

      
    }
}
