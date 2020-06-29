using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Infrastructure.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Infrastructure.Persistence.InMemory
{
    public class PaymentRequestRepository : IPaymentRequestRepository
    {

        private CheckItOutContext _context;

        public PaymentRequestRepository(CheckItOutContext context)
        {
            _context = context;
        }

        public async Task Add(PaymentRequest entity)
        {
            await _context.AddAsync(entity);
        }

        public async Task<PaymentRequest> Get(string paymentRequestId)
        {
            return await _context.PaymentRequests.FirstOrDefaultAsync(x => x.PaymentRequestId == paymentRequestId);
        }

        public async Task<PaymentRequest> GetById(string paymentRequestId)
        {
            return await _context.PaymentRequests.FindAsync(paymentRequestId);
        }

        public async Task<PaymentRequest> GetByInvoiceId(string invoiceId)
        {
            return await _context.PaymentRequests.FirstOrDefaultAsync(x => x.InvoiceId == invoiceId);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
