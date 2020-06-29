using CheckItOut.Payments.Domain.MerchantContracts;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Infrastructure.Merchant
{
    public class NotifyMerchantPaymentSubmitted : INotifyMerchantPaymentSubmitted
    {
        public async Task Notify()
        {
            string invoiceId = "";
            string paymentStatus = "submitted";
            HttpClient httpClient = new HttpClient();
            await httpClient.PostAsync("https://localhost:44388/Orders/Notifications", new StringContent(JsonConvert.SerializeObject(new { invoiceId, paymentStatus }),Encoding.UTF8));
        }
    }
}
