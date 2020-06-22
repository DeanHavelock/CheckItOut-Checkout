using CheckItOut.Payments.Domain.Commands;
using CheckItOut.Payments.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CheckItOut.Payments.Domain.Interfaces;
using System.Threading.Tasks;
using CheckItOut.Payments.Domain.Interfaces.Repository;

namespace CheckItOut.Payments.Application.CommandHandlers
{    
    public class PaymentsCommandHandler : IPaymentsCommandHandler
    {
        private IPaymentRepository _paymentRepository;

        public PaymentsCommandHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }


        public async Task Process(MakePaymentCommand command)
        {
            var payment = new Payment
            {
                Id = command.PaymentId,
                MerchantId = command.MerchantId,
                Amount = command.Amount,
                CardNumber = command.CardNumber                
            };

            await _paymentRepository.Add(payment);
            await _paymentRepository.Save();
        }
    }
}
