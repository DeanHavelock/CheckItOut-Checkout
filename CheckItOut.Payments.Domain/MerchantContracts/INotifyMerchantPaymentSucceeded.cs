using System.Threading.Tasks;

namespace CheckItOut.Payments.Domain.MerchantContracts
{
    public  interface INotifyMerchantPaymentSucceeded
    {
        Task Notify();
    }
}