using System;

using System.Threading.Tasks;
using CheckItOut.Payments.Api.Dtos;
using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Commands;
using CheckItOut.Payments.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CheckItOut.Payments.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private IPaymentsCommandHandler _paymentCommandHandler;
        private readonly CheckItOutContext _checkitOutContext;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IPaymentsCommandHandler paymentCommandHandler/*ILogger<PaymentsController> logger*/, CheckItOutContext checkitOutContext)
        {
            _paymentCommandHandler = paymentCommandHandler;
            _checkitOutContext = checkitOutContext;
            //_logger = logger;
        }

        [HttpGet(Name = "GetPayment")]
        public async Task<IActionResult> Get(Guid paymentId)
        {            
           
            return Ok();
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

          //  await _paymentCommandHandler.Process(command);

            var u = Url.Link("GetPayment", new { paymentId = paymentId   });

            return Created(u, null);            
        }
    }
}
