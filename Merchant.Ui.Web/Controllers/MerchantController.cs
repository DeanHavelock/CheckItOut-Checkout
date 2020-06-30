using Merchant.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Merchant.Ui.Web.Controllers
{
    public class MerchantController : Controller
    {
        private IQueryOrdersApplicationService _queryOrdersApplicationService;

        public MerchantController(IQueryOrdersApplicationService queryOrdersApplicationService)
        {
            _queryOrdersApplicationService = queryOrdersApplicationService;
        }

        [HttpGet, Route("Merchant")]
        public IActionResult Index()
        {
            var merchantId = "TEST";
            var viewModel = _queryOrdersApplicationService.GetMerchantOrdersAsync(merchantId).Result;

            return View(viewModel);
        }
    }
}
