using Merchant.Application.Clients.CheckItOut.Payments.Dtos;
using Merchant.Domain;
using Merchant.Domain.HttpContracts;
using Merchant.Domain.Interfaces;
using System;
using System.Data;
using System.Linq;
using System.Security;

namespace Merchant.Application
{
    public class CheckoutApplicationService : ICheckoutApplicationService
    {
        private IQueryCheckoutApplicationService _queryCheckoutApplicationService;
        private IOrderRepository _orderRepository;
        private readonly IPostToSecureHttpEndpointWithRetries _postToSecureHttpEndpointWithRetries;
        private void IdempotentVerificationCheck(string invoiceId)
        {
            var duplicateOrderCheck = _orderRepository.GetByInvoiceId(invoiceId);
            if (duplicateOrderCheck != null && !string.IsNullOrWhiteSpace(duplicateOrderCheck.InvoiceId))
                throw new VerificationException("duplicate order with invoiceId: " + invoiceId + " detected, preventing duplicate order.");
        }

        public CheckoutApplicationService(IQueryCheckoutApplicationService queryCheckoutApplicationService, IOrderRepository orderRepository, IPostToSecureHttpEndpointWithRetries postToSecureHttpEndpointWithRetries)
        {
            _queryCheckoutApplicationService = queryCheckoutApplicationService;
            _orderRepository = orderRepository;
            _postToSecureHttpEndpointWithRetries = postToSecureHttpEndpointWithRetries;
        }

        public string NotPciDssCheckoutSendCardDetailsFromMerchant(string invoiceId, string userId, string senderCardNumber, string senderCvv, string senderCardExpiryMonth, string senderCardExpiryYear)
        {
            IdempotentVerificationCheck(invoiceId);
            var checkout = _queryCheckoutApplicationService.GetCheckoutFromBasket(userId);
            var orderId = Guid.NewGuid().ToString();
            Order order = new Order() { OrderId = orderId, InvoiceId=invoiceId, UserId = userId, MerchantId = checkout.SellerMerchantId, Status = OrderStatus.Ordered, CurrencyCode = checkout.CurrencyCode, OrderItems = checkout.CheckoutProductViewModels.ToList().Select(x=> new OrderItem { OrderItemId = Guid.NewGuid().ToString(), OrderId=orderId, Title = x.Title, Price=x.Price }) } ;
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

        public string PciDssCheckout(string invoiceId, string userId)
        {
            IdempotentVerificationCheck(invoiceId);
            var checkout = _queryCheckoutApplicationService.GetCheckoutFromBasket(userId);
            var orderId = Guid.NewGuid().ToString();
            Order order = new Order() { OrderId = orderId, InvoiceId = invoiceId, UserId = userId, Status = OrderStatus.Ordered, CurrencyCode = checkout.CurrencyCode, MerchantId=checkout.SellerMerchantId, OrderItems = checkout.CheckoutProductViewModels.ToList().Select(x => new OrderItem { OrderItemId = Guid.NewGuid().ToString(), OrderId = orderId, Title = x.Title, Price = x.Price }) };
            _orderRepository.Add(order);

            var dto = new PrepairGuestPaymentRequest() { InvoiceId = invoiceId, Amount = Convert.ToString(checkout.TotalCost), CurrencyCode = checkout.CurrencyCode, OrderId = order.OrderId, RecipientMerchantId=checkout.SellerMerchantId, UserId=userId };
            var response = _postToSecureHttpEndpointWithRetries.Post(apiClientUrl: "https://localhost:44379/prepairpayment", idServerUrl: "https://localhost:5001", "client", "secret", "CheckoutApi", dto);
            var deserialisedContentPaymentRequestId = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                order.Status = OrderStatus.Ordered;
                _orderRepository.Update(order);
            }

            return deserialisedContentPaymentRequestId;
        }

        public void PciDssFinaliseOrder(string invoiceId, string paymentId)
        {
            if (!string.IsNullOrWhiteSpace(paymentId))
            {
                var order = _orderRepository.GetByInvoiceId(invoiceId);
                order.Paid(paymentId);
                _orderRepository.Update(order);
            }
        }
    }
}
