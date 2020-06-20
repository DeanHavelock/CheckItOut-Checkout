using System;

namespace CheckItOut.Payments.Domain
{
    public class Payment
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }
    }
}
