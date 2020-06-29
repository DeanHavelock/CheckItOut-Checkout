using CheckItOut.Payments.Domain.BankSim;
using CheckItOut.Payments.Domain.BankSim.Dto;
using CheckItOut.Payments.Domain.HttpContracts;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Infrastructure.BankSim
{
    public class ChargeCardAdapter : IChargeCardAdapter
    {
        private IPostToSecureHttpEndpointWithRetries _postToSecureHttpEndpointWithRetries;
        public ChargeCardAdapter(IPostToSecureHttpEndpointWithRetries postToSecureHttpEndpointWithRetries)
        {
            _postToSecureHttpEndpointWithRetries = postToSecureHttpEndpointWithRetries;
        }

        public Task<FinaliseTransactionResponse> Charge(FinaliseTransactionRequest dto)
        {
            var response = _postToSecureHttpEndpointWithRetries.Post(apiClientUrl: "https://localhost:44345/BankPayment", idServerUrl: "https://localhost:5001", "bankSimClient", "secret", "BankSimApi", dto);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var deserializedResponseContent = JsonConvert.DeserializeObject<FinaliseTransactionResponse>(responseContent);
            //if (response.IsSuccessStatusCode)
            return Task.FromResult(deserializedResponseContent);
        }
    }
}
