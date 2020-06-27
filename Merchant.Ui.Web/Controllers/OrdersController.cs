using System;
using Merchant.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Merchant.Ui.Web.Controllers
{
    public class OrdersController : Controller
    {
        private IQueryCheckoutApplicationService _queryCheckoutApplicationService;
        private ICheckoutApplicationService _checkoutApplicationService;

        //public OrdersController(IQueryCheckoutApplicationService queryCheckoutApplicationService, ICheckoutApplicationService checkoutApplicationService)
        //{
        //    _queryCheckoutApplicationService = queryCheckoutApplicationService;
        //    _checkoutApplicationService = checkoutApplicationService;
        //}

        //[Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var subjectIdFromContext = "2b837f52-becd-4938-8a35-0906d8c7d591";
            var checkout = _queryCheckoutApplicationService.GetCheckoutFromBasket(subjectIdFromContext);
            return View(checkout);
        }

        //[Authorize]
        [HttpPost, Route("Orders")]
        public IActionResult Pay()
        {
            string subjectId = "2b837f52-becd-4938-8a35-0906d8c7d591"; //Get from signed in authentication context (subject)
            _checkoutApplicationService.Checkout(subjectId);
            return RedirectToAction("OrderSubmitted");
        }

        [HttpGet]
        public IActionResult OrderSubmitted()
        {
            return new ContentResult() { Content = "Order Submitted - " + Guid.NewGuid().ToString(), StatusCode = 201 };
        }

    }
}
