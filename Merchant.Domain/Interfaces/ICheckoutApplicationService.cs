using Merchant.Domain.Clients.CheckItOutPayments.Dtos;
using System.Collections.Generic;

namespace Merchant.Domain.Interfaces
{
    public interface ICheckoutApplicationService
    {
        string PciDssCheckout(string invoiceId, string userId);
        void PciDssFinaliseOrder(string invoiceId, string paymentId);
        string NotPciDssCheckoutSendCardDetailsFromMerchant(string invoiceId, string userId, string senderCardNumber, string senderCvv, string senderCardExpiryMonth, string senderCardExpiryYear);
    }
}
