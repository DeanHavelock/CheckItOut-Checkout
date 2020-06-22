using CheckItOut.Payments.Domain.Queries.Projections;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Domain.Queries
{
    public interface IQueryPayments
    {
        Task<GetPaymentResponse> Query(GetPayment query);
    }
}
