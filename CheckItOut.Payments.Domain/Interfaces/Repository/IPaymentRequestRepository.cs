using System.Threading.Tasks;

namespace CheckItOut.Payments.Domain.Interfaces.Repository
{
    public interface IPaymentRequestRepository
    {
        Task<PaymentRequest> Get(string paymentRequestId);
        Task Add(PaymentRequest entity);
        Task Save();
        Task<PaymentRequest> GetById(string paymentRequestId);
        Task<PaymentRequest> GetByInvoiceId(string invoiceId);
    }
}
