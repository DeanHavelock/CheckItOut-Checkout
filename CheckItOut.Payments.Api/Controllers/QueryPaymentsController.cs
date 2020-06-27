using CheckItOut.Payments.Domain.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Api.Controllers
{
    [Route("payments")]
    public class QueryPaymentsController : ControllerBase
    {
        private IQueryPayments _paymentsQuery;

        public QueryPaymentsController(IQueryPayments paymentsQuery)
        {
            _paymentsQuery = paymentsQuery;
        }        

        [Authorize]
        [HttpGet, Route("{paymentId}", Name = "GetPayment")]
        public async Task<IActionResult> Get(string paymentId)
        {
            var payment = await _paymentsQuery.Query(new GetPayment { PaymentId = paymentId });

            return Ok(payment);
        }
    }
}
