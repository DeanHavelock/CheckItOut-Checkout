using Merchant.Domain.ViewModels;

namespace Merchant.Domain.Interfaces
{
    public interface IQueryCheckoutApplicationService
    {
        CheckoutViewModel GetCheckoutFromBasket(string userId);
    }
}
