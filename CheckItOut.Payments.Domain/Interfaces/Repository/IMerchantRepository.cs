using System;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Domain.Interfaces.Repository
{
    public interface IMerchantRepository
    {
        Task Add(Merchant entity);
        Task Save();
        Task<Merchant> GetById(string id);
    }
}
