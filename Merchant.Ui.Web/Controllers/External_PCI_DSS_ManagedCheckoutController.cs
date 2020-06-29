using Microsoft.AspNetCore.Mvc;

namespace Merchant.Ui.Web.Controllers
{
    [Route("PCI-Dss-ManagedCheckout")]
    public class External_PCI_DSS_ManagedCheckoutController : Controller
    {
        public External_PCI_DSS_ManagedCheckoutController()
        {
            //_checkoutApplicationService = checkoutApplicationService;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        //ToDo: [Authorize(AuthenticationSchemes="CheckItOutPaymentAuthenticationScheme")]//Authorise Endpoint to only allow Client from CheckItOut.Payment to call
        //[HttpPost]
        //public void Notifications(string invoiceId, string sendAddress, string paymentId, string merchantId, string currencyCode, string amount, IEnumerable<OrderedItem> orderedItems)
        //{
        //    var a = "";
        //    //successful payment received, Check if Order exists from invoiceId, if none existing Create Order, set paymentStatus on Order and Order.PaymentId 
        //    //_checkoutApplicationService.CheckoutUsingCardDetailsAndBasketFromExternalSite(invoiceId, sendAddress, paymentId, merchantId, currencyCode, amount, orderedItems);
        //}

    }
}
