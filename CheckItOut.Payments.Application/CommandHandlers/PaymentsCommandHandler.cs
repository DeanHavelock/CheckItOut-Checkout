using CheckItOut.Payments.Domain.Commands;
using CheckItOut.Payments.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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


        public async Task Process(MakePaymentCommand command)
        {
            var merchant = await _merchantQueries.GetById(command.MerchantId);

            var chargeRequest = new FinaliseTransactionRequest
            {
                RecipientAccountNumber = merchant.AccountNumber,
                SenderCardNumber = command.CardNumber
            };

            //MakePayment
            var chargeResponse = await _chargeCard.Charge(chargeRequest);

            //Save
            var payment = new Payment
            {
                Id = command.PaymentId,
                MerchantId = command.MerchantId,
                Amount = command.Amount,
                CardNumber = command.CardNumber,
                TransactionId = chargeResponse.TransactionId
            };

            await _paymentRepository.Add(payment);
            await _paymentRepository.Save();
        }
    }
}
