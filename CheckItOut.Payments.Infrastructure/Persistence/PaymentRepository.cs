using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Infrastructure.Persistence.InMemory
{
    public class PaymentRepository : IPaymentRepository
    {
        public Task Add(Payment payment)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }
    }
}
