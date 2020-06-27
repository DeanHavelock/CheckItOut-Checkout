using System;
using System.Collections.Generic;
using System.Text;

namespace CheckItOut.Payments.Domain.Queries.Projections
{
    public class GetPaymentResponse
    {
        public decimal Amount { get; set; }
        public string PaymentId { get; set; }
    }
}
