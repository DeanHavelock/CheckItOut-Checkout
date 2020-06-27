using System;
using System.Collections.Generic;
using System.Text;

namespace Merchant.Domain
{
    public class OrderItem
    {
        public string OrderItemId { get; set; }
        public string OrderId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
