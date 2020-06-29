namespace Merchant.Domain.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Order order);
        void Update(Order order);
        Order GetByInvoiceId(string invoiceId);
    }
}
