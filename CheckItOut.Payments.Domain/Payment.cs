using System;

namespace CheckItOut.Payments.Domain
{
    public class Payment
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string MerchantId { get; set; }
        public string TransactionId { get; set; }
    }
}
