using CheckItOut.Payments.Domain.BankSim;
using CheckItOut.Payments.Domain.BankSim.Dto;
using System;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Infrastructure.BankSim
{
    public class ChargeCard : IChargeCard
    {
        public Task<FinaliseTransactionResponse> Charge(FinaliseTransactionRequest finaliseTransactionRequest)
        {
            //    HttpClient client = new HttpClient();
            //    var response = await client.PostAsync("https://banksim.com/ChargeCard", new StringContent(JsonConvert.SerializeObject(makePaymentRequest), Encoding.UTF8, "application/json")

            return Task.FromResult(new FinaliseTransactionResponse { BankSimTransactionId = Guid.NewGuid().ToString() });
        }
    }
}
