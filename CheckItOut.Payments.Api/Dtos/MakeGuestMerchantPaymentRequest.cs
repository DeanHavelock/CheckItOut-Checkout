namespace CheckItOut.Payments.Api.Dtos
{
    public class MakeGuestToMerchantPaymentRequest
    {
        public string OrderId { get; set; }
        public string InvoiceId { get; set; }
        public string RecipientMerchantId { get; set; }
        public string SenderCardNumber { get; set; }
        public string SenderCvv { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string UserId { get; set; }
        public string SenderCardExpiryYear { get; set; }
        public string SenderCardExpiryMonth { get; set; }
    }
}