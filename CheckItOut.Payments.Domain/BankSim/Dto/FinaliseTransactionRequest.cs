namespace CheckItOut.Payments.Domain.BankSim.Dto
{
    public class FinaliseTransactionRequest
    {
        //sender
        public string SenderCardNumber { get; set; }
        public string SenderCsv { get; set; }

        //receiver
        public string RecipientFullName { get; set; }
        public string RecipientAccountNumber { get; set; }
        public string RecipientSortCode { get; set; }
    }
}
