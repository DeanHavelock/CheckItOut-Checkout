using Merchant.Domain.Interfaces;
using Merchant.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Merchant.Ui.Web.Controllers
{
    public partial class PciDssCheckoutController : Controller
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
        [HttpPost, Route("Checkout/Post")]
        public IActionResult Post(string invoiceId)
        {
            string userId = "2b837f52-becd-4938-8a35-0906d8c7d591"; //Get from signed in user authentication context (subjectId)
            var paymentRequestId = _checkoutApplicationService.PciDssCheckout(invoiceId, userId);

            return Redirect("https://localhost:44328/TakePayment?paymentRequestId=" + paymentRequestId);
        }

        //[Authorize]
        [HttpGet, Route("CheckoutResponse")]
        public IActionResult CheckoutResponse(string invoiceId, string paymentId)
        {
            _checkoutApplicationService.PciDssFinaliseOrder(invoiceId, paymentId);
            return View(new CheckoutOrderPlacedViewModel() { InvoiceId = invoiceId, PaymentId = paymentId, MerchantUrl = "https://localhost:44388/merchant" });
        }

    }
}
