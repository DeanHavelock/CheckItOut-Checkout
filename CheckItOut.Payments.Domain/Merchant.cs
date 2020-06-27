namespace CheckItOut.Payments.Domain
{
    public class Merchant
    {
        public string MerchantId { get; set; }
        public string AccountNumber { get; set; }
        public string SortCode { get; set; }
        public string FullName { get; set; }
        public string CardNumber { get; set; }
        public string Csv { get; set; }
    }
}
