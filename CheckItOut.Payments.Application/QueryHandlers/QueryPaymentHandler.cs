using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Domain.Queries;
using CheckItOut.Payments.Domain.Queries.Projections;
using System;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Application.QueryHandlers
{
    public class QueryPaymentHandler : IQueryPayments
    {
        private readonly IPaymentRepository _paymentRepository;

        public QueryPaymentHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }       

        public async Task<GetPaymentResponse> Query(GetPayment query)
        {
            var payment = await _paymentRepository.GetById(query.PaymentId);
            var paymentQueryResponse = MapResponseFromEntity(payment);
            return paymentQueryResponse;
        }

        private GetPaymentResponse MapResponseFromEntity(Payment payment)
        {
            return new GetPaymentResponse 
            { 
                PaymentId = payment.PaymentId, 
                Amount = payment.Amount,
                MaskedCardNumber = payment.SenderCardNumber,
                Succeeded = (payment.Status == PaymentStatus.Succeeded)
            };
        }
    }
}
