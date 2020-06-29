using Merchant.Domain.Clients.CheckItOutPayments.Dtos;
using System.Collections.Generic;

namespace Merchant.Domain.Interfaces
{
    public interface ICheckoutApplicationService
    {
        string Checkout(string invoiceId, string userId);
        void FinaliseOrder(string invoiceId, string paymentId);
        string CheckoutUsingCardDetailsAndBasketFromMerchantSite(string invoiceId, string userId, string senderCardNumber, string senderCvv, string senderCardExpiryMonth, string senderCardExpiryYear);
        void CheckoutUsingCardDetailsAndBasketFromExternalSite(string invoiceId, string sendAddress, string paymentId, string merchantId, string currencyCode, string amount, IEnumerable<OrderedItem> orderedItems);
    }
}
