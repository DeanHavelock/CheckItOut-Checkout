using System.Net.Http;
using System.Threading.Tasks;

namespace Merchant.Domain.HttpContracts
{
    public interface IGetToSecureHttpEndpointWithRetries {
        Task<HttpResponseMessage> Get(string apiClientUrl, string idServerUrl, string clientId, string secret, string tokenScope);
    }
}
