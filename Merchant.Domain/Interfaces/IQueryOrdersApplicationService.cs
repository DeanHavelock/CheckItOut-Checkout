using Merchant.Domain.ViewModels;
using System.Threading.Tasks;

namespace Merchant.Domain.Interfaces
{
    public interface IQueryOrdersApplicationService
    {
        Task<MerchantOrdersViewModel> GetMerchantOrdersAsync(string merchantId);
    }
}
