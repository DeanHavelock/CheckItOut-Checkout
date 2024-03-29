﻿using Merchant.Domain;
using Merchant.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Merchant.Infrastructure.Persistence.EntityFramework
{
    public class OrderRepository : IOrderRepository
    {
        private WebAppDbContext _webAppDbContext;

        public OrderRepository(WebAppDbContext webAppDbContext)
        {
            _webAppDbContext = webAppDbContext;
        }

        public void Add(Order order)
        {
            _webAppDbContext.Orders.Add(order);
            _webAppDbContext.OrderItems.AddRange(order.OrderItems);
            _webAppDbContext.SaveChanges();
        }

        public void Update(Order order)
        {
            var matchingOrder = _webAppDbContext.Orders.First(x => x.OrderId == order.OrderId);
            matchingOrder.Status = order.Status;
            matchingOrder.PaymentId = order.PaymentId;
            _webAppDbContext.Orders.Update(matchingOrder);
            _webAppDbContext.SaveChanges();
        }

        public Order GetByInvoiceId(string invoiceId)
        {
            var order = _webAppDbContext.Orders.FirstOrDefault(x => x.InvoiceId == invoiceId);
            if (order == null)
                return new Order();
            order.OrderItems = _webAppDbContext.OrderItems.Where(x => x.OrderId == order.OrderId).ToList();
            return order;
        }

        public IEnumerable<Order> GetAllByMerchantId(string merchantId)
        {
            var orders = _webAppDbContext.Orders.Where(x => x.MerchantId == merchantId).ToList();
            if (orders == null)
                return new List<Order>();
            foreach(var order in orders)
            {
                order.OrderItems = _webAppDbContext.OrderItems.Where(x => x.OrderId == order.OrderId).ToList();
            }
            return orders;
        }
    }
}
