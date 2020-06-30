using CheckItOut.Payments.Domain.Commands;
using CheckItOut.Payments.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckItOut.Payments.Api.Controllers
{
    [Route("prepairpayment")]
    public class PrepairPaymentController : ControllerBase
    {
        private IPrepairPaymentCommandHandler _prepairPaymentCommandHandler;

        public PrepairPaymentController(IPrepairPaymentCommandHandler prepairPaymentCommandHandler)
        {
            _prepairPaymentCommandHandler = prepairPaymentCommandHandler;
        }

        public IActionResult Get(string paymentRequestId)
        {
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody]PrepairGuestPaymentRequest command)
        {
            var paymentRequestId = _prepairPaymentCommandHandler.Handle(command);
            return Created("https://localhost:44379/prepairpayment", paymentRequestId);
        }

    } 
}
