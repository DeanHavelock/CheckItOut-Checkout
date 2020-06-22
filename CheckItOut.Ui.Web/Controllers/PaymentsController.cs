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

        
        // POST: PaymentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PaymentsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PaymentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PaymentsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PaymentsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
