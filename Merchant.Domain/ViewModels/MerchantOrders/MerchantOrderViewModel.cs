using System.Collections.Generic;

namespace Merchant.Domain.ViewModels
{
    public class MerchantOrderViewModel
    {
        public MerchantOrderViewModel()
        {
            MerchentOrderProductViewModels = new List<MerchantOrderProductViewModel>();
        }
        public string InvoiceId { get; set; }
        public string OrderStatus { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public IEnumerable<MerchantOrderProductViewModel> MerchentOrderProductViewModels { get; set; }
        public string PaymentId { get; set; }
        public string MaskedCardNumber { get; set; }
        public bool ProviderVerifiedPaymentStatus { get; set; }
    }
}
