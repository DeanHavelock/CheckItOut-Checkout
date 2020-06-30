using IdentityModel.Client;
using Merchant.Domain.HttpContracts;
using System;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Merchant.Infrastructure.HttpSecureSender
{
    public class GetToSecureHttpEndpointWithRetries : IGetToSecureHttpEndpointWithRetries
    {
        public Task<HttpResponseMessage> Get(string apiClientUrl, string idServerUrl, string clientId, string secret, string tokenScope)
        {
            var response = GetToSecurePaymentApiWithRetries(apiClientUrl: apiClientUrl, idServerUrl: idServerUrl, clientId, secret, tokenScope);

            return response;
        }

        private Task<HttpResponseMessage> GetToSecurePaymentApiWithRetries(string apiClientUrl, string idServerUrl, string clientId, string secret, string tokenScope)
        {
            var tokenResponse = RequestJWTokenFromIdServer(idServerUrl, clientId, secret, tokenScope);
            var response = GetToSecureApiWithRetries(apiClientUrl, tokenResponse);
            return response;
        }

        private Task<HttpResponseMessage> GetToSecureApiWithRetries(string apiClientUrl, TokenResponse tokenResponse)
        {
            //httpGet to apiClientUrl endpoint with jwtBearerToken and retries
            HttpClient apiClient = HttpClientFactory.Create(new RetryHandler());
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var response = apiClient.GetAsync(apiClientUrl);
            if (!response.Result.IsSuccessStatusCode)
            {
                throw new Exception(apiClientUrl + " statusCode: " + response.Result.StatusCode.ToString());
            }
            return response;
        }

        private TokenResponse RequestJWTokenFromIdServer(string idServerUrl, string clientId, string secret, string tokenScope)
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
