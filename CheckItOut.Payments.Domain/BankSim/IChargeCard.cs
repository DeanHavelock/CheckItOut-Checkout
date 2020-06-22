using System;
using System.Collections.Generic;
using System.Text;

namespace CheckItOut.Payments.Domain.BankSim
{
    public interface IChargeCard
    {
        void Charge();
    }
}
