using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Domain.Queries;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Application.QueryHandlers
{
    public class QueryMerchantsHandler : IQueryMerchants
    {
        private readonly IMerchantRepository _merchantRepository;

        public QueryMerchantsHandler(IMerchantRepository merchantRepository)
        {
            _merchantRepository = merchantRepository;
        }

        public async Task<Merchant> GetById(string id)
        {
            var result = await _merchantRepository.GetById(id);
            return result;
        }
    }
}
