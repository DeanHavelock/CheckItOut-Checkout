namespace Merchant.Domain.Interfaces
{
    public interface ICheckoutApplicationService
    {
        string Checkout(string invoiceId, string userId, string senderCardNumber, string senderCvv, string senderCardExpiryMonth, string senderCardExpiryYear);
    }
}
