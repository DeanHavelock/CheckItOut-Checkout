using System;
using System.Collections.Generic;
using System.Text;

namespace CheckItOut.Payments.Domain
{
    public class PaymentRequest
    {
        public string PaymentRequestId { get; set; }
        public string InvoiceId { get; set; }
        public string MerchantId { get; set; }
        public string Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}
