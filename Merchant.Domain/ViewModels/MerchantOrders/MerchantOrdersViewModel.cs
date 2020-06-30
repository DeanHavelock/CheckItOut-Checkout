using System.Collections.Generic;

namespace Merchant.Domain.ViewModels
{
    public class MerchantOrdersViewModel
    {
        public MerchantOrdersViewModel()
        {
            MerchantOrders = new List<MerchantOrderViewModel>();
        }
        public string MerchantId { get; set; }
        public IEnumerable<MerchantOrderViewModel> MerchantOrders { get; set; }
    }
}
