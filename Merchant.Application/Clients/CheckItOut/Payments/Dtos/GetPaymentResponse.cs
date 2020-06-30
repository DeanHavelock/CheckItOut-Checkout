namespace CheckItOut.Payments.Domain.Queries.Projections
{
    public class GetPaymentResponse
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string MaskedCardNumber { get; set; }
        public string PaymentId { get; set; }
        public bool Succeeded { get; set; }
    }
}
