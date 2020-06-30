using System.Collections.Generic;

namespace Merchant.Domain.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Order order);
        void Update(Order order);
        Order GetByInvoiceId(string invoiceId);
        IEnumerable<Order> GetAllByMerchantId(string merchantId);
    }
}
