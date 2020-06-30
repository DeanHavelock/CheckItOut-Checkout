using System;
using System.Collections.Generic;
using System.Linq;

namespace Merchant.Domain.ViewModels
{
    public class CheckoutViewModel
    {
        public CheckoutViewModel()
        {
            InvoiceId = Guid.NewGuid().ToString();
            OrderId = Guid.NewGuid().ToString();
            UserId = "2b837f52-becd-4938-8a35-0906d8c7d591";
            SellerMerchantId = "TEST";
            SellerName = "SellItAll";
            CurrencyCode = "GBP";
            CheckoutProductViewModels = new List<CheckoutProductViewModel>()
            {
                new CheckoutProductViewModel{ Title="Classic White T Shirt Size M", Price=24.99m, Delivery=0.00m},
                new CheckoutProductViewModel{ Title="Classic Oxford Shirt Size M", Price=24.99m, Delivery=0.00m}
            };
        }

        public IEnumerable<CheckoutProductViewModel> CheckoutProductViewModels;
        public string SellerName { get; }
        public string SellerMerchantId { get; }
        public decimal TotalCost => CheckoutProductViewModels.Sum(x => x.Price + x.Delivery);
        public string CurrencyCode { get; }

        public string UserId { get; }
        public string InvoiceId { get; }
        public string OrderId { get; }
    }
}
