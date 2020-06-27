using System;
using System.Threading.Tasks;
using CheckItOut.Payments.Api.Dtos;
using CheckItOut.Payments.Domain.Commands;
using CheckItOut.Payments.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckItOut.Payments.Api.Controllers
{
    [ApiController]
    [Route("payments")]
    public class PaymentsController : ControllerBase
    {
        private IPaymentsCommandHandler _paymentCommandHandler;

        public PaymentsController(IPaymentsCommandHandler paymentCommandHandler)
        {
            _paymentCommandHandler = paymentCommandHandler;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(MakePaymentRequest paymentRequest)
        {
            //ToDo, Implement Idempotency with InvoiceId
            var command = MakeCommand(paymentRequest);

            await _paymentCommandHandler.Process(command);

            var uri = Url.Link("GetPayment", new { paymentId = command.PaymentId   });

            return Created(uri, null);            
        }

        //place in factory
        private static MakePaymentCommand MakeCommand(MakePaymentRequest request)
        {
            var paymentId = Guid.NewGuid();

            var command = new MakePaymentCommand
            {
                PaymentId = paymentId,
                Amount = request.Amount,
                CardNumber = request.CardNumber,
                MerchantId = "TEST"
            };

            return command;
        }
    }
}
