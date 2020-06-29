using CheckItOut.Payments.Api.Models;
using CheckItOut.Payments.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CheckItOut.Payments.Api.Controllers
{
    public class CheckoutController : Controller
    {
        private IPaymentsCommandHandler _paymentCommandHandler;

        public CheckoutController(IPaymentsCommandHandler paymentCommandHandler)
        {
            _paymentCommandHandler = paymentCommandHandler;
        }

        [HttpGet]
        public ActionResult Create(string invoiceId = "", string orderId = "", string amount = "", string recipientMerchantId = "", string currencyCode="GBP")
        {
            var viewModel = new CreatePaymentViewModel() { InvoiceId = invoiceId, OrderId = orderId, Amount = amount, RecipientMerchantId=recipientMerchantId, CurrencyCode=currencyCode};
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Post(string senderCardNumber, string senderCvv, string senderCardExpiryMonth, string senderCardExpiryYear, string invoiceId = "", string orderId = "", string amount = "0", string recipientMerchantId = "", string currencyCode = "GBP")
        {
            _paymentCommandHandler.Handle(new Domain.Commands.MakePaymentCommand { Amount = Convert.ToDecimal(amount), CurrencyCode = currencyCode, InvoiceId = invoiceId, OrderId = orderId, RecipientMerchantId = recipientMerchantId, SenderCardNumber = senderCardNumber, SenderCvv = senderCvv, SenderCardExpiryMonth = senderCardExpiryMonth, SenderCardExpiryYear = senderCardExpiryYear });
            return Content("Order Submitted, invoiceId: " + invoiceId);
        }
    }
}
