using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Infrastructure.Persistence.EntityFramework;
using System;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Infrastructure.Persistence.InMemory
{
    public class MerchantRepository : IMerchantRepository
    {

        private CheckItOutContext _context;

        public MerchantRepository(CheckItOutContext context)
        {
            _context = context;
        }

        public async Task Add(Merchant entity)
        {
            await _context.AddAsync(entity);
        }


        public async Task<Merchant> GetById(string id)
        {
            return await _context.Merchants.FindAsync(id);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
