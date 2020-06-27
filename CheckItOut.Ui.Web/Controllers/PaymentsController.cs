using System;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckItOut.Ui.Web.Controllers
{

    public class PaymentsController : Controller
    {
        // GET: PaymentsController
        [Authorize]
        public ActionResult Index()
        {
            using var client = new HttpClient();

            var result = client.GetAsync("https://localhost:44379/payments").Result;
            Console.WriteLine(result.StatusCode);

           return View();
        }

    }
}
