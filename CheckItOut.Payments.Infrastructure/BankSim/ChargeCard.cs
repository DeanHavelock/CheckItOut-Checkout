using CheckItOut.Payments.Domain.BankSim;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CheckItOut.Payments.Infrastructure.BankSim
{
    public class ChargeCard : IChargeCard
    {
        //public void Card(FromCard fromCard, ToAccount toAccount)
        //{
        //    HttpClient client = new HttpClient();
        //    client.PostAsync("https://banksim.com/ChargeCard", new StringContent(JsonConvert.SerializeObject(makePaymentRequest), Encoding.UTF8, "application/json")
        //}
        public void Charge()
        {
            throw new NotImplementedException();
        }
    }
}
