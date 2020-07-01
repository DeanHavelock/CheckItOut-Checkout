using IdentityModel.Client;
using Merchant.Domain.HttpContracts;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace Merchant.Infrastructure.HttpSecureSender
{
    public class PostToSecureHttpEndpointWithRetries : CallToSecureHttpEndpointWithRetriesBase, IPostToSecureHttpEndpointWithRetries
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
            //posts to apiClientUrl endpoint with jwtBearerToken and retries
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

    }
}
