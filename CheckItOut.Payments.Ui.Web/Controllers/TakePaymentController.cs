using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Commands;
using CheckItOut.Payments.Domain.Interfaces;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Ui.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CheckItOut.Payments.Ui.Web.Controllers
{
    public class TakePaymentController : Controller
    {
        private IPaymentRequestRepository _paymentRequestRepository;
        private IPaymentsCommandHandler _paymentsCommandHandler;

        public TakePaymentController(IPaymentRequestRepository paymentRequestRepository, IPaymentsCommandHandler paymentsCommandHandler)
        {
            _paymentRequestRepository = paymentRequestRepository;
            _paymentsCommandHandler = paymentsCommandHandler;
        }

        [HttpGet]
        public IActionResult Index(string paymentRequestId)
        {
            var paymentRequest = _paymentRequestRepository.Get(paymentRequestId).Result;
            var createPaymentViewModel = new CreatePaymentViewModel() { InvoiceId= paymentRequest.InvoiceId, Amount=paymentRequest.Amount, CurrencyCode=paymentRequest.CurrencyCode, RecipientMerchantId=paymentRequest.MerchantId, PaymentRequestId=paymentRequestId };
            AddTestData(createPaymentViewModel);
            return View(createPaymentViewModel);

        }

        private void AddTestData(CreatePaymentViewModel createPaymentViewModel)
        {
            createPaymentViewModel.SenderFullName = "dr bob smith";
            createPaymentViewModel.SenderCardNumber = "1234123412341234";
            createPaymentViewModel.SenderCardCvv = "123";
            createPaymentViewModel.SenderCardExpiryMonth = "11";
            createPaymentViewModel.SenderCardExpiryYear = "2022";
        }

        [HttpPost]
        public IActionResult Post(CreatePaymentViewModel viewModel)
        {
            var paymentRequest = _paymentRequestRepository.Get(viewModel.PaymentRequestId).Result;
            var makePaymentCommand = MapFrom(paymentRequest, viewModel);

            var paymentId = _paymentsCommandHandler.Handle(makePaymentCommand).Result;

            return Redirect("https://localhost:44388/CheckoutResponse?invoiceId=" + viewModel.InvoiceId+"&paymentId="+paymentId);
        }

        private MakePaymentCommand MapFrom(PaymentRequest paymentRequest, CreatePaymentViewModel viewModel)
        {
            return new MakePaymentCommand() { InvoiceId = paymentRequest.InvoiceId, RecipientMerchantId = paymentRequest.MerchantId, Amount = Convert.ToDecimal(paymentRequest.Amount), CurrencyCode = paymentRequest.CurrencyCode, SenderCardNumber = viewModel.SenderCardNumber, SenderCvv = viewModel.SenderCardCvv, SenderCardExpiryMonth = viewModel.SenderCardExpiryMonth, SenderCardExpiryYear = viewModel.SenderCardExpiryYear, SenderFullName= viewModel.SenderFullName };
        }
    }
}
