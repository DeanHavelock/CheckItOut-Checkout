namespace Merchant.Domain.ViewModels
{
    public class CheckoutOrderPlacedViewModel
    {
        public string InvoiceId { get; set; }
        public string PaymentId { get; set; }
        public string MerchantUrl { get; set; }
    }
}
