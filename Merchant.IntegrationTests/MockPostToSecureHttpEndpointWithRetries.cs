using Merchant.Domain.HttpContracts;
using System.Net.Http;

namespace Merchant.IntegrationTests
{
    public class MockPostToSecureHttpEndpointWithRetries : IPostToSecureHttpEndpointWithRetries
    {
        public HttpResponseMessage Post(string apiClientUrl, string idServerUrl, string clientId, string secret, string tokenScope, object dto)
        {
            return new HttpResponseMessage() { };
        }  
    }
}
