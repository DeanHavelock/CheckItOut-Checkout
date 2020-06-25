using System;
using System.ComponentModel.DataAnnotations;

namespace CheckItOut.Payments.Api.Dtos
{
    public class MakePaymentRequest
    {
        public string InvoiceId { get; set; }

        [Range(1, 100000)]
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string CardNumber { get; set; }
    }
}
