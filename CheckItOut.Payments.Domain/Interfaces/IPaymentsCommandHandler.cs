using CheckItOut.Payments.Domain.Commands;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Domain.Interfaces
{
    public interface IPaymentsCommandHandler
    {
        public Task Process(MakePaymentCommand command);
    }
}
