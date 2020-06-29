using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.MerchantContracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Infrastructure.Merchant
{
    public class NotifyMerchantPaymentSucceeded : INotifyMerchantPaymentSucceeded
    {
        public async Task Notify(string invoiceId, string sendAddress, string paymentId, string merchantId, string currencyCode, string amount, IEnumerable<OrderedItem> orderedItems)
        {
            string paymentStatus = "paid";
            HttpClient httpClient = new HttpClient();
            await httpClient.PostAsync("https://localhost:44388/Orders/Notifications", new StringContent(JsonConvert.SerializeObject(new { invoiceId, paymentId, merchantId, paymentStatus }), Encoding.UTF8));
        }
    }
}
