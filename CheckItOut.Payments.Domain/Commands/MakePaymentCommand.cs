using System;

namespace CheckItOut.Payments.Domain.Commands
{
    public class MakePaymentCommand
    {
        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string MerchantId { get; set; }
    }
}
