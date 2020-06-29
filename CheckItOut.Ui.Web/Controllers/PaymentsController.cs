using System;
using System.Net.Http;
using CheckItOut.Payments.Domain.Commands;
using CheckItOut.Payments.Domain.Interfaces;
using CheckItOut.Ui.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckItOut.Ui.Web.Controllers
{

    public class PaymentsController : Controller
    {
        private IPaymentsCommandHandler _paymentsCommandHandler;

        public PaymentsController(IPaymentsCommandHandler paymentsCommandHandler)
        {
            _paymentsCommandHandler = paymentsCommandHandler;
        }
        // GET: PaymentsController
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            using var client = new HttpClient();

            var result = client.GetAsync("https://localhost:44379/payments").Result;
            Console.WriteLine(result.StatusCode);

           return View();
        }

        [HttpGet]
        public ActionResult Create(string invoiceId="", string orderId="", string amount="", string recipientMerchantId="")
        {
            var viewModel = new CreatePaymentViewModel() { InvoiceId = invoiceId, OrderId = orderId, Amount = amount };


            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Post(string invoiceId)
        {
            var command = new MakePaymentCommand();
            _paymentsCommandHandler.Handle(command);

            return Content("Order Submitted, invoiceId: " + invoiceId);
        }

    }
}
