using System.Threading.Tasks;

namespace CheckItOut.Payments.Domain.Queries
{
    public interface IQueryMerchants
    {
        Task<Merchant> GetById(string merchantId);
    }
}
