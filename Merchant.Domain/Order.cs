using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Merchant.Domain
{
    public class Order
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public OrderStatus Status { get; set; }
        public string CurrencyCode { get; set; }
        public double TotalAmount { get { return OrderItems.Sum(x=>x.Price); } }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
