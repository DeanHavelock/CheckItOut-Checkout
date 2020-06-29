using Merchant.Application.Clients.CheckItOut.Payments.Dtos;
using Merchant.Domain;
using Merchant.Domain.Clients.CheckItOutPayments.Dtos;
using Merchant.Domain.HttpContracts;
using Merchant.Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public string CheckoutUsingCardDetailsAndBasketFromMerchantSite(string invoiceId, string userId, string senderCardNumber, string senderCvv, string senderCardExpiryMonth, string senderCardExpiryYear)
        {
            var checkout = _queryCheckoutApplicationService.GetCheckoutFromBasket(userId);
            var orderId = Guid.NewGuid().ToString();
            Order order = new Order() { OrderId = orderId, InvoiceId=invoiceId, UserId = userId, Status = OrderStatus.Ordered, CurrencyCode = checkout.CurrencyCode, OrderItems = checkout.CheckoutProductViewModels.ToList().Select(x=> new OrderItem { OrderItemId = Guid.NewGuid().ToString(), OrderId=orderId, Title = x.Title, Price=x.Price }) } ;
            _orderRepository.Add(order);

            var dto = new MakeGuestToMerchantPaymentRequest { OrderId= orderId, InvoiceId = invoiceId, UserId = order.UserId, RecipientMerchantId = checkout.SellerMerchantId, Amount = checkout.TotalCost, CurrencyCode = checkout.CurrencyCode, SenderCardNumber=senderCardNumber, SenderCvv=senderCvv, SenderCardExpiryMonth=senderCardExpiryMonth, SenderCardExpiryYear=senderCardExpiryYear };

            var response = _postToSecureHttpEndpointWithRetries.Post(apiClientUrl: "https://localhost:44379/Payments", idServerUrl: "https://localhost:5001", "client", "secret", "CheckoutApi", JsonConvert.SerializeObject(dto));
            
            if (response.IsSuccessStatusCode)
            {
                order.Status = OrderStatus.Paid;
                _orderRepository.Update(order);
            }

            return order.OrderId;
        }

        public void CheckoutUsingCardDetailsAndBasketFromExternalSite(string invoiceId, string sendAddress, string paymentId, string merchantId, string currencyCode, string amount, IEnumerable<OrderedItem> orderedItems)
        {
            Order order;
            var existingOrder = _orderRepository.GetByInvoiceId(invoiceId);
            if (existingOrder == null)
            {
                var newOrder = new Order() { OrderId = Guid.NewGuid().ToString(), SendAddress=sendAddress, InvoiceId = invoiceId, CurrencyCode = currencyCode, MerchantId=merchantId };
                var newOrderItems = MapFrom(orderedItems, newOrder.OrderId);
                newOrder.OrderItems = newOrderItems;
                order = newOrder;
                _orderRepository.Add(order);
            }
            else
            {
                order = existingOrder;
            }
            order.Paid(paymentId);
            _orderRepository.Update(order);
        }

        private IEnumerable<OrderItem> MapFrom(IEnumerable<OrderedItem> orderedItems, string orderId)
        {
            var orderItems = orderedItems
                .Select(x => new OrderItem() { Title = x.Title, Price = Convert.ToDecimal(x.Amount) })
                .ToList();
            return orderItems;
        }

        public string Checkout(string invoiceId, string userId)
        {
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

        public void FinaliseOrder(string invoiceId, string paymentId)
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
