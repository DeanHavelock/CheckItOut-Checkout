using System;
using System.Linq;
using System.Threading.Tasks;
using CheckItOut.Payments.Api.Dtos;
using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Commands;
using CheckItOut.Payments.Domain.Interfaces;
using CheckItOut.Payments.Infrastructure.Persistence.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CheckItOut.Payments.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private IPaymentsCommandHandler _paymentCommandHandler;
        private CheckItOutContext _checkItOutContext;

        public PaymentsController(IPaymentsCommandHandler paymentCommandHandler, CheckItOutContext checkitOutContext)
        {
            _paymentCommandHandler = paymentCommandHandler;
            _checkItOutContext = checkitOutContext;
        }

        [HttpGet(Name = "GetPayment")]
        public async Task<IActionResult> Get(Guid paymentId)
        {
            var allPayments = _checkItOutContext.Payments.ToList();
            return Ok(allPayments);
        }

        [HttpPost]
        public async Task<IActionResult> Post(MakePaymentRequest paymentRequest)
        {
            var paymentId = Guid.NewGuid();

            var command = new MakePaymentCommand
            {
                PaymentId = paymentId,
                Amount = paymentRequest.Amount
            };

            await _paymentCommandHandler.Process(command);

            var u = Url.Link("GetPayment", new { paymentId = paymentId   });

            return Created(u, null);            
        }
    }
}
