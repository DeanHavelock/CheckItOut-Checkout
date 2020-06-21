using CheckItOut.Payments.Api.Controllers;
using CheckItOut.Payments.Api.Dtos;
using CheckItOut.Payments.Application.CommandHandlers;
using CheckItOut.Payments.Domain.Commands;
using CheckItOut.Payments.Domain.Interfaces;
using CheckItOut.Payments.Infrastructure.Persistence.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CheckItOut.Payments.UnitTests
{
    public class PaymentControllerTests
    {
        [Fact]
        public async Task ShouldProcessCommand()
        {
            var commandHander = new Mock<IPaymentsCommandHandler>();

            var controller = new PaymentsController(commandHander.Object, new CheckItOutContext(null));

            var request = new MakePaymentRequest { Amount = 99.89m };

            await controller.Post(request);

            commandHander.Verify(handler => handler.Process(It.Is<MakePaymentCommand>((command) => command.Amount == 99.89m)));
        }

       
    }
}
