using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Infrastructure.Persistence.EntityFramework;
using System;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Infrastructure.Persistence.InMemory
{
    public class PaymentRepository : IPaymentRepository
    {

        private CheckItOutContext _context;

        public PaymentRepository(CheckItOutContext context)
        {
            _context = context;
        }

        public async Task Add(Payment payment)
        {
            await _context.AddAsync(payment);
        }

        public async Task<Payment> GetById(Guid paymentId)
        {
            return await _context.Payments.FindAsync(paymentId);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
