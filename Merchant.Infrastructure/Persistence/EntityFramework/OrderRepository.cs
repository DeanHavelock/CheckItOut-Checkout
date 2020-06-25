using Merchant.Domain;
using Merchant.Domain.Interfaces;
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
            _webAppDbContext.Orders.Update(matchingOrder);
            _webAppDbContext.SaveChanges();
        }
    }
}
