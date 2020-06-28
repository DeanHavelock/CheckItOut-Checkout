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
        public async Task<IActionResult> Post(MakeGuestToMerchantPaymentRequest paymentRequest)
        {
            //ToDo, Implement Idempotency with InvoiceId
            var command = MakeCommand(paymentRequest);

            await _paymentCommandHandler.Handle(command);

            var uri = Url.Link("GetPayment", new { paymentId = command.PaymentId });

            return Created(uri, null);
        }

        ////move to factory
        private static MakePaymentCommand MakeCommand(MakeGuestToMerchantPaymentRequest request)
        {
            var paymentId = Guid.NewGuid().ToString();

            var command = new MakePaymentCommand
            {
                PaymentId = paymentId,
                InvoiceId = request.InvoiceId,
                OrderId = request.OrderId,
                Amount = request.Amount,
                CurrencyCode= request.CurrencyCode,
                RecipientMerchantId = request.RecipientMerchantId,
                SenderCardNumber = request.SenderCardNumber,
                SenderCvv = request.SenderCvv,
                SenderCardExpiryMonth = request.SenderCardExpiryMonth,
                SenderCardExpiryYear = request.SenderCardExpiryYear,
                UserId = request.UserId
            };
        
            return command;
        }

    }
}
