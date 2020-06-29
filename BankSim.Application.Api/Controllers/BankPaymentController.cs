using BankSim.Application.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BankSim.Application.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankPaymentController : ControllerBase
    {

        [HttpGet]
        public object Get(string bankSimTransactionId)
        {
            return new { bankSimTransactionId, success=true };
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post(FinaliseTransactionRequest dto)
        {
            var paymentReponse = MockProcessPayment(dto);
            string url = "https://localhost:44345/BankPayment?bankSimTransactionId=" + paymentReponse.BankSimTransactionId;
            return Created(url, paymentReponse);
        }

        private FinaliseTransactionResponse MockProcessPayment(FinaliseTransactionRequest dto)
        {
            // idempotency check 

            // encapsulate entity setup and validationChecks
            //payment.Setup(values)

            //process payment
            return new FinaliseTransactionResponse() { BankSimTransactionId = Guid.NewGuid().ToString(), Success = true };
        }
    }
}
