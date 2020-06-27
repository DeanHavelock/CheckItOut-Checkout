using IdentityModel.Client;
using Merchant.Domain;
using Merchant.Domain.Interfaces;
using Merchant.Domain.ViewModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;

namespace Merchant.Application
{
    public class CheckoutApplicationService : ICheckoutApplicationService
    {
        private IQueryCheckoutApplicationService _queryCheckoutApplicationService;
        private IOrderRepository _orderRepository;

        public CheckoutApplicationService(IQueryCheckoutApplicationService queryCheckoutApplicationService, IOrderRepository orderRepository)
        {
            _queryCheckoutApplicationService = queryCheckoutApplicationService;
            _orderRepository = orderRepository;
        }

        public string Checkout(string userId)
        {
            var checkout = _queryCheckoutApplicationService.GetCheckoutFromBasket(userId);
            var orderId = Guid.NewGuid().ToString();
            Order order = new Order() { OrderId = orderId, UserId = userId, Status = OrderStatus.Ordered, CurrencyCode = checkout.CurrencyCode, OrderItems = checkout.CheckoutProductViewModels.ToList().Select(x=> new OrderItem { OrderItemId = Guid.NewGuid().ToString(), OrderId=orderId, Title = x.Title, Price=x.Price }) } ;
            _orderRepository.Add(order);

            var response = PostToSecurePaymentApiWithRetries(apiClientUrl:"https://localhost:44379/Payments", idServerUrl:"https://localhost:5001", "CheckoutApi", checkout, order);

            if (response.IsSuccessStatusCode)
            {
                order.Status = OrderStatus.Paid;
                _orderRepository.Update(order);
            }

            return order.OrderId;
        }
        
        private HttpResponseMessage PostToSecurePaymentApiWithRetries(string apiClientUrl, string idServerUrl, string tokenScope, CheckoutViewModel checkout, Order order)
        {
            var tokenResponse = RequestJWTokenFromIdServer(idServerUrl, tokenScope);
            var response = PostToSecureApiWithRetries(apiClientUrl, tokenResponse, checkout, order);
            return response;
        }

        private HttpResponseMessage PostToSecureApiWithRetries(string apiClientUrl, TokenResponse tokenResponse, CheckoutViewModel checkout, Order order)
        {
            //posts to CheckItOut idempotent payment endpoint with retries
            HttpClient apiClient = HttpClientFactory.Create(new RetryHandler());
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var content = new StringContent(JsonConvert.SerializeObject(new { invoiceId = order.OrderId, order.UserId, checkout.SellerMerchantId, checkout.TotalCost, currencyCode = "GBP" }), Encoding.UTF8, "application/json");
            var response = apiClient.PostAsync(apiClientUrl, content).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(apiClientUrl + " statusCode: " + response.StatusCode.ToString());
            }
            return response;
        }

        private TokenResponse RequestJWTokenFromIdServer(string idServerUrl, string tokenScope)
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

                ClientId = "client",
                ClientSecret = "secret",
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
