using IdentityModel.Client;
using System.Net.Http;
using System.Security.Authentication;

namespace Merchant.Infrastructure.HttpSecureSender
{
    public abstract class CallToSecureHttpEndpointWithRetriesBase
    {
        protected TokenResponse RequestJWTokenFromIdServer(string idServerUrl, string clientId, string secret, string tokenScope)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync(idServerUrl).Result;
            if (disco.IsError)
            {
                throw new AuthenticationException(disco.Error);
            }

            // request token
            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = clientId,
                ClientSecret = secret,
                Scope = tokenScope
            }).Result;

            if (tokenResponse.IsError)
            {
                throw new AuthenticationException(tokenResponse.Error);
            }

            return tokenResponse;
        }
    }
}
