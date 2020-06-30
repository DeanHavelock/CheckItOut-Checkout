using System;

namespace CheckItOut.Payments.Domain
{
    public class Payment
    {
        private string _cardNumber;

        public Payment()
        {
            Status = PaymentStatus.Pending;
        }

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
                if (value.Length >= 12)
                {
                    var l4 = value.Substring(12, 4);
                    _cardNumber = "############" + l4;
                }
            }
        }

        public string CurrencyCode { get; set; }

        public string Status { get; set; }
        public string OrderId { get; set; }

        public void Succeed(string transactionId)
        {
            BankSimTransactionId = transactionId;
            Status = PaymentStatus.Succeeded;
        }
        public void Fail(string reason)
        {
            Status = PaymentStatus.Failed;
        }

    }

    public static class PaymentStatus
    {
        public const string Pending = "pending";
        public const string Succeeded = "succeeded";
        public const string Failed = "failed";
    }
}
