using CheckItOut.Payments.Domain.Commands;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Domain.Interfaces
{
    public interface IFullyManagedCheckoutPaymentCommandHandler
    {
        public Task Handle(MakeFullyManagedBasketCheckoutPaymentCommand command);
    }
}
