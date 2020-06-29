using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Merchant.Domain
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public string OrderId { get; set; }
        public string InvoiceId { get; set; }
        public string UserId { get; set; }
        public string MerchantId { get; set; }
        public string SendAddress { get; set; }
        public OrderStatus Status { get; set; }
        public string CurrencyCode { get; set; }
        public decimal TotalAmount { get { return OrderItems.Sum(x=>x.Price); } }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        public string PaymentId { get; set; }

        public void Paid(string paymentId)
        {
            Status=OrderStatus.Paid;
            PaymentId = paymentId;
        }
    }
}
