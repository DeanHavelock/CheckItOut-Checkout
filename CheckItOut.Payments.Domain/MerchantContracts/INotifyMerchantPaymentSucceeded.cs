using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Domain.MerchantContracts
{
    public  interface INotifyMerchantPaymentSucceeded
    {
        Task Notify(string invoiceId, string sendAddress, string paymentId, string merchantId, string currencyCode, string amount, IEnumerable<OrderedItem> orderedItems);
    }
}