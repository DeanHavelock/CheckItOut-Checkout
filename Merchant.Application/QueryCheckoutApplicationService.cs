using Merchant.Domain.Interfaces;
using Merchant.Domain.ViewModels;

namespace Merchant.Application
{
    public class QueryCheckoutApplicationService : IQueryCheckoutApplicationService
    {
        public CheckoutViewModel GetCheckoutFromBasket(string userId)
        {
            return new CheckoutViewModel();
        }
    }
}
