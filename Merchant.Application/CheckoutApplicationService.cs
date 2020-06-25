using Merchant.Domain;
using Merchant.Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
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

            //posts to CheckItOut idempotent payment endpoint with retries
            HttpClient client = HttpClientFactory.Create(new RetryHandler());
            var content = new StringContent(JsonConvert.SerializeObject(new { invoiceId = orderId, userId, checkout.SellerMerchantId, checkout.TotalCost, currencyCode = "GBP" }), Encoding.UTF8, "application/json");
            var response = client.PostAsync("https://localhost:44379/Payments", content).Result;

            if (response.IsSuccessStatusCode)
            {
                order.Status = OrderStatus.Paid;
                _orderRepository.Update(order);
            }

            return orderId;
        }
    }
}
