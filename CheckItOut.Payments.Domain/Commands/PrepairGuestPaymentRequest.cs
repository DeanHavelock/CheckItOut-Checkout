namespace CheckItOut.Payments.Domain.Commands
{
    public class PrepairGuestPaymentRequest
    {
        public string PaymentRequestId {get;set;}
        public string InvoiceId { get; set; }
        public string OrderId { get; set; }
        public string Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string RecipientMerchantId { get; set; }
        public string UserId { get; set; }
    }

}
