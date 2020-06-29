using System.Threading.Tasks;

namespace CheckItOut.Payments.Domain.MerchantContracts
{
    public interface INotifyMerchantPaymentSubmitted
    {
        Task Notify();
    }
}