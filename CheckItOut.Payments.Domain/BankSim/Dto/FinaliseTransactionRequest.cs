namespace CheckItOut.Payments.Domain.BankSim.Dto
{
    public class FinaliseTransactionRequest
    {
        public string InvoiceId { get; set; }

        //sender
        public string SenderCardNumber { get; set; }
        public string SenderCvv { get; set; }

        //receiver
        public string RecipientFullName { get; set; }
        public string RecipientAccountNumber { get; set; }
        public string RecipientSortCode { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentId { get; set; }
        public string SenderCardExpiryMonth { get; set; }
        public string SenderCardExpiryYear { get; set; }
    }
}
