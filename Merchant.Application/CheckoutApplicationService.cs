using Merchant.Application.Clients.CheckItOut.Payments.Dtos;
using Merchant.Domain;
using Merchant.Domain.HttpContracts;
using Merchant.Domain.Interfaces;
using System;
using System.Linq;

namespace Merchant.Application
{
    public class CheckoutApplicationService : ICheckoutApplicationService
    {
        private IQueryCheckoutApplicationService _queryCheckoutApplicationService;
        private IOrderRepository _orderRepository;
        private readonly IPostToSecureHttpEndpointWithRetries _postToSecureHttpEndpointWithRetries;

        public CheckoutApplicationService(IQueryCheckoutApplicationService queryCheckoutApplicationService, IOrderRepository orderRepository, IPostToSecureHttpEndpointWithRetries postToSecureHttpEndpointWithRetries)
        {
            _queryCheckoutApplicationService = queryCheckoutApplicationService;
            _orderRepository = orderRepository;
            _postToSecureHttpEndpointWithRetries = postToSecureHttpEndpointWithRetries;
        }

        public string Checkout(string invoiceId, string userId, string senderCardNumber, string senderCvv, string senderCardExpiryMonth, string senderCardExpiryYear)
        {
            var checkout = _queryCheckoutApplicationService.GetCheckoutFromBasket(userId);
            var orderId = Guid.NewGuid().ToString();
            Order order = new Order() { OrderId = orderId, UserId = userId, Status = OrderStatus.Ordered, CurrencyCode = checkout.CurrencyCode, OrderItems = checkout.CheckoutProductViewModels.ToList().Select(x=> new OrderItem { OrderItemId = Guid.NewGuid().ToString(), OrderId=orderId, Title = x.Title, Price=x.Price }) } ;
            _orderRepository.Add(order);

            var dto = new MakeGuestToMerchantPaymentRequest { OrderId= orderId, InvoiceId = invoiceId, UserId = order.UserId, RecipientMerchantId = checkout.SellerMerchantId, Amount = checkout.TotalCost, CurrencyCode = checkout.CurrencyCode, SenderCardNumber=senderCardNumber, SenderCvv=senderCvv, SenderCardExpiryMonth=senderCardExpiryMonth, SenderCardExpiryYear=senderCardExpiryYear };

            var response = _postToSecureHttpEndpointWithRetries.Post(apiClientUrl: "https://localhost:44379/Payments", idServerUrl: "https://localhost:5001", "client", "secret", "CheckoutApi", dto);
            
            if (response.IsSuccessStatusCode)
            {
                order.Status = OrderStatus.Paid;
                _orderRepository.Update(order);
            }

            return order.OrderId;
        }
    }
}
