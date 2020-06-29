using CheckItOut.Payments.Domain.BankSim.Dto;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Domain.BankSim
{
    public interface IChargeCardAdapter
    {
        Task<FinaliseTransactionResponse> Charge(FinaliseTransactionRequest finaliseTransactionRequest);
    }
}
