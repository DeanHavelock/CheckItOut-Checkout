using CheckItOut.Payments.Domain.BankSim.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Domain.BankSim
{
    public interface IChargeCard
    {
        Task<FinaliseTransactionResponse> Charge(FinaliseTransactionRequest finaliseTransactionRequest);
    }
}
