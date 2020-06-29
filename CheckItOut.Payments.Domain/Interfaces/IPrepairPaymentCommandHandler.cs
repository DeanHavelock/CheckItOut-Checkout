using CheckItOut.Payments.Domain.Commands;

namespace CheckItOut.Payments.Domain.Interfaces
{
    public interface IPrepairPaymentCommandHandler
    {
        string Handle(PrepairGuestPaymentRequest command);
    }
}