using IdentityModel.Client;
using Merchant.Domain.HttpContracts;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;

namespace Merchant.Infrastructure.HttpSecureSender
{
    public class PostToSecureHttpEndpointWithRetries : IPostToSecureHttpEndpointWithRetries
    {
        public HttpResponseMessage Post(string apiClientUrl, string idServerUrl, string clientId, string secret, string tokenScope, object dto)
        {
            var response = PostToSecurePaymentApiWithRetries(apiClientUrl: apiClientUrl, idServerUrl: idServerUrl, clientId, secret, tokenScope, dto);

            return response;
        }
        
        private HttpResponseMessage PostToSecurePaymentApiWithRetries(string apiClientUrl, string idServerUrl, string clientId, string secret, string tokenScope, object dto)
        {
            var tokenResponse = RequestJWTokenFromIdServer(idServerUrl, clientId, secret, tokenScope);
            var response = PostToSecureApiWithRetries(apiClientUrl, tokenResponse, dto);
            return response;
        }

        private HttpResponseMessage PostToSecureApiWithRetries(string apiClientUrl, TokenResponse tokenResponse, object dto)
        {
            //posts to CheckItOut idempotent payment endpoint with retries
            HttpClient apiClient = HttpClientFactory.Create(new RetryHandler());
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = apiClient.PostAsync(apiClientUrl, content).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(apiClientUrl + " statusCode: " + response.StatusCode.ToString());
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
