﻿using Merchant.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Merchant.Ui.Web.Controllers
{
    public class PciDssCheckoutController : Controller
    {
        private IQueryCheckoutApplicationService _queryCheckoutApplicationService;
        private ICheckoutApplicationService _checkoutApplicationService;

        public PciDssCheckoutController(IQueryCheckoutApplicationService queryCheckoutApplicationService, ICheckoutApplicationService checkoutApplicationService)
        {
            _queryCheckoutApplicationService = queryCheckoutApplicationService;
            _checkoutApplicationService = checkoutApplicationService;
        }

        [Route("Checkout")]
        //[Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var subjectIdFromContext = "2b837f52-becd-4938-8a35-0906d8c7d591";
            var checkout = _queryCheckoutApplicationService.GetCheckoutFromBasket(subjectIdFromContext);
            return View(checkout);
        }

        //[Authorize]
        [HttpPost, Route("Checkout")]
        public IActionResult Post(string invoiceId)
        {
            string userId = "2b837f52-becd-4938-8a35-0906d8c7d591"; //Get from signed in user authentication context (subjectId)
            var paymentRequestId = _checkoutApplicationService.Checkout(invoiceId, userId);

            return Redirect("https://localhost:44328/TakePayment?paymentRequestId=" + paymentRequestId);
        }

        //[Authorize]
        [HttpGet, Route("CheckoutResponse")]
        public IActionResult CheckoutResponse(string invoiceId, string paymentId)
        {
            _checkoutApplicationService.FinaliseOrder(invoiceId, paymentId);
            return new ContentResult() { Content = "Order Placed: " + invoiceId, StatusCode = 201 };
        }

    }
}