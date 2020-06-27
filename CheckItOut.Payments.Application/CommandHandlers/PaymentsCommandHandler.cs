using CheckItOut.Payments.Domain.Commands;
using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Interfaces;
using System.Threading.Tasks;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Domain.BankSim;
using CheckItOut.Payments.Domain.BankSim.Dto;
using CheckItOut.Payments.Domain.Queries;

namespace CheckItOut.Payments.Application.CommandHandlers
{
    public class PaymentsCommandHandler : IPaymentsCommandHandler
    {
        private IPaymentRepository _paymentRepository;
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
            //ToDo: Idempotent InvoiceId Check Here
            //.......

            var recipient = await _merchantQueries.GetById(command.RecipientMerchantId);
            var chargeRequest = new FinaliseTransactionRequest
            {
                InvoiceId = command.InvoiceId,
                RecipientAccountNumber = recipient.AccountNumber,
                RecipientSortCode = recipient.SortCode,
                SenderCardNumber = command.SenderCardNumber,
                SenderCvv = command.SenderCvv,
                Amount = command.Amount,
                CurrencyCode = command.CurrencyCode,
            };
            
            //Save
            var payment = new Payment
            {
                PaymentId = command.PaymentId,
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

            payment.BankSimTransactionId = chargeResponse.BankSimTransactionId;

            //await _paymentRepository.Add(payment);
            await _paymentRepository.Save();
        }

        //public async Task Process(MakeMerchantToMerchantPaymentCommand command)
        //{
        //    var sender = await _merchantQueries.GetById(command.SenderMerchantId);
        //    var recipient = await _merchantQueries.GetById(command.RecipientMerchantId);

        //    var chargeRequest = new FinaliseTransactionRequest
        //    {
        //        InvoiceId = command.InvoiceId,
        //        RecipientAccountNumber = recipient.AccountNumber,
        //        RecipientSortCode = recipient.SortCode,
        //        SenderCardNumber = sender.CardNumber,
        //        SenderCsv = sender.Csv
        //    };

        //    //MakePayment
        //    var chargeResponse = await _chargeCard.Charge(chargeRequest);

        //    //Save
        //    var payment = new Payment
        //    {
        //        PaymentId = command.PaymentId,
        //        InvoiceId = command.InvoiceId,
        //        SenderMerchantId = sender.MerchantId,
        //        CardNumber = sender.CardNumber,
        //        Amount = command.Amount,
        //        RecipientMerchantId = recipient.MerchantId,
        //        BankSimTransactionId = chargeResponse.TransactionId
        //    };

        //    await _paymentRepository.Add(payment);
        //    await _paymentRepository.Save();
        //}
    }
}
