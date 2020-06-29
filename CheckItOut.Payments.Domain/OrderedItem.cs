namespace CheckItOut.Payments.Domain
{
    public class OrderedItem
    {
        public string OrderedItemId { get; set; }
        public string PaymentId { get; set; }
        public string Amount { get; set; }
        public string Title { get; set; }
    }
}