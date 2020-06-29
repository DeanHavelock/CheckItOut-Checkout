using CheckItOut.Payments.Domain.Interfaces;
using CheckItOut.Payments.Ui.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CheckItOut.Payments.Ui.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private IFullyManagedCheckoutPaymentCommandHandler _paymentsCommandHandler;

        public CheckoutController(IFullyManagedCheckoutPaymentCommandHandler paymentsCommandHandler)
        {
            _paymentsCommandHandler = paymentsCommandHandler;
        }

        [HttpGet]
        public ActionResult Get(string senderEmail = "test@customeremail.com", string amount = "0", string recipientMerchantId = "", string currencyCode = "GBP")
        {
            var viewModel = new CreatePaymentViewModel() { InvoiceId = Guid.NewGuid().ToString(), Amount = amount, RecipientMerchantId = recipientMerchantId, CurrencyCode = currencyCode };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Post( string senderCardNumber, string senderCvv, string senderCardExpiryMonth, string senderCardExpiryYear, string invoiceId = "", string amount = "0", string recipientMerchantId = "", string currencyCode = "GBP", string senderEmail = "test@customeremail.com")
        {
            _paymentsCommandHandler.Handle(new Domain.Commands.MakeFullyManagedBasketCheckoutPaymentCommand { SenderEmail = senderEmail, Amount = Convert.ToDecimal(amount), CurrencyCode = currencyCode, InvoiceId = invoiceId, RecipientMerchantId = recipientMerchantId, SenderCardNumber = senderCardNumber, SenderCvv = senderCvv, SenderCardExpiryMonth = senderCardExpiryMonth, SenderCardExpiryYear = senderCardExpiryYear });
            return Content("Order Submitted, invoiceId: " + invoiceId);
        }
    }
}
