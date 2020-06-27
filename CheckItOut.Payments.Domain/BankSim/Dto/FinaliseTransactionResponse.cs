using System;
using System.Collections.Generic;
using System.Text;

namespace CheckItOut.Payments.Domain.BankSim.Dto
{
   public  class FinaliseTransactionResponse
    {
        public string BankSimTransactionId { get; set; }
        public bool Success { get; set; }
    }
}
