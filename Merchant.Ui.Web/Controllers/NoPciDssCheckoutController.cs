using System;
using Merchant.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Merchant.Ui.Web.Controllers
{
    //[Route("Payment")]
    public class NoPciDssCheckoutController : Controller
    {
        private IQueryCheckoutApplicationService _queryCheckoutApplicationService;
        private ICheckoutApplicationService _checkoutApplicationService;

        public NoPciDssCheckoutController(IQueryCheckoutApplicationService queryCheckoutApplicationService, ICheckoutApplicationService checkoutApplicationService)
        {
            _queryCheckoutApplicationService = queryCheckoutApplicationService;
            _checkoutApplicationService = checkoutApplicationService;
        }

        //[Authorize]
        [HttpGet, Route("Payment")]
        public IActionResult Index()
        {
            var subjectIdFromContext = "2b837f52-becd-4938-8a35-0906d8c7d591";
            var checkout = _queryCheckoutApplicationService.GetCheckoutFromBasket(subjectIdFromContext);
            return View(checkout);
        }

        //[Authorize]
        [HttpPost, Route("Payment/Post")]
        public IActionResult CheckoutUsingCardDetailsAndBasketFromMerchantSite(string invoiceId)
        {
            string subjectId = "2b837f52-becd-4938-8a35-0906d8c7d591"; //Get from signed in user authentication context (subject)
            string senderCardNumber = "1234123412341234";
            string senderCvv = "111";
            string senderCardExpiryMonth = "02";
            string senderCardExpiryYear = "2020";
            var orderId = _checkoutApplicationService.NotPciDssCheckoutSendCardDetailsFromMerchant(invoiceId, subjectId, senderCardNumber, senderCvv, senderCardExpiryMonth, senderCardExpiryYear);
            return RedirectToAction("OrderSubmitted", new { orderId });
        }

        [HttpPost, Route("PaymentOrderSubmitted")]
        public IActionResult OrderSubmitted(string orderId)
        {
            return new ContentResult() { Content = "Order Submitted - " + orderId, StatusCode = 201 };
        }
    }
}
