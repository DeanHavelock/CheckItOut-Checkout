using System.Net.Http;

namespace Merchant.Domain.HttpContracts
{
    public interface IPostToSecureHttpEndpointWithRetries {
        HttpResponseMessage Post(string apiClientUrl, string idServerUrl, string clientId, string secret, string tokenScope, object dto);
    }
}
