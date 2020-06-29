using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.BankSim;
using CheckItOut.Payments.Domain.BankSim.Dto;
using CheckItOut.Payments.Domain.Commands;
using CheckItOut.Payments.Domain.Interfaces;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Domain.MerchantContracts;
using CheckItOut.Payments.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Application.CommandHandlers
{
    public class FullyManagedCheckoutPaymentCommandHandler : IFullyManagedCheckoutPaymentCommandHandler
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IChargeCard _chargeCard;
        private readonly IQueryMerchants _merchantQueries;
        private readonly INotifyMerchantPaymentSucceeded _notifyMerchantPaymentSucceeded;

        public FullyManagedCheckoutPaymentCommandHandler(IPaymentRepository paymentRepository, IQueryMerchants merchantQueries, IChargeCard chargeCard, INotifyMerchantPaymentSucceeded notifyMerchantPaymentSucceeded)
        {
            _paymentRepository = paymentRepository;
            _chargeCard = chargeCard;
            _merchantQueries = merchantQueries;
            _notifyMerchantPaymentSucceeded = notifyMerchantPaymentSucceeded;
        }

        public async Task Handle(MakeFullyManagedBasketCheckoutPaymentCommand command)
        {
            var duplicatePaymentAttempt = _paymentRepository.GetByInvoiceId(command.InvoiceId).Result;
            if (duplicatePaymentAttempt != null && !string.IsNullOrWhiteSpace(duplicatePaymentAttempt.InvoiceId))
                throw new Exception("Duplicate Payment Attempt, PaymentId with InvoiceId: " + command.InvoiceId + " already exists");

            //Prepaire ChargeRequest with Sender And Merchant Details
            var recipient = _merchantQueries.GetById(command.RecipientMerchantId).Result;
            var chargeRequest = MapToChargeRequest(command, recipient);

            //Save Payment Request
            var payment = MapToPayment(command, recipient);
            _paymentRepository.Add(payment).Wait();
            _paymentRepository.Save().Wait();

            //ToDo: Implement Retries and Auth to Endpoint
            //MakePayment
            var chargeResponse = await _chargeCard.Charge(chargeRequest);
            if (chargeResponse.Success)
            {
                payment.Succeed(chargeResponse.BankSimTransactionId);
                _paymentRepository.Save().Wait();
                await _notifyMerchantPaymentSucceeded.Notify(command.InvoiceId, "customer.sendAddress", payment.PaymentId, recipient.MerchantId, command.CurrencyCode, "amount", new List<OrderedItem> { });
                //await _notifyCustomerByEmailPaymentSucceeded.Notify(command.SenderEmail, command.InvoiceId, payment.PaymentId, recipient.MerchantId);
            }
            else
            {
                payment.Fail("return a reason from bank");
                _paymentRepository.Save().Wait();
            }
        }

        private Payment MapToPayment(MakeFullyManagedBasketCheckoutPaymentCommand command, Merchant recipient)
        {
            return new Payment
            {
                PaymentId = Guid.NewGuid().ToString(),
                OrderId = command.OrderId,
                InvoiceId = command.InvoiceId,
                RecipientMerchantId = recipient.MerchantId,
                SenderCardNumber = command.SenderCardNumber,
                Amount = command.Amount,
                CurrencyCode = command.CurrencyCode,
            };
        }

        private FinaliseTransactionRequest MapToChargeRequest(MakeFullyManagedBasketCheckoutPaymentCommand command, Merchant recipient)
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
