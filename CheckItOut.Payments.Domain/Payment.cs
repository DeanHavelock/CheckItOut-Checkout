using System;

namespace CheckItOut.Payments.Domain
{
    public class Payment
    {
        private string _cardNumber;

        public string PaymentId { get; set; }
        public string InvoiceId { get; set; }
        public string BankSimTransactionId { get; set; }
        public decimal Amount { get; set; }

        public string RecipientMerchantId { get; set; }
        public string SenderCardNumber
        {
            get { return _cardNumber; }
            set
            {
                var l4 = value.Substring(12, 4);
                _cardNumber = "############" + l4;
            }
        }

        public string CurrencyCode { get; set; }
    }
}
