using System;
using System.Collections.Generic;
using System.Linq;

namespace Merchant.Domain.ViewModels
{
    public class CheckoutViewModel
    {
        public CheckoutViewModel()
        {
            UserId = "2b837f52-becd-4938-8a35-0906d8c7d591";
            SellerMerchantId = Guid.NewGuid().ToString();
            SellerName = "SellItAll";
            CurrencyCode = "GBP";
            CheckoutProductViewModels = new List<CheckoutProductViewModel>()
            {
                new CheckoutProductViewModel{ Title="Classic White T Shirt Size M", Price=24.99, Delivery=0.00},
                new CheckoutProductViewModel{ Title="Classic Oxford Shirt Size M", Price=24.99, Delivery=0.00}
            };
        }

        public IEnumerable<CheckoutProductViewModel> CheckoutProductViewModels;
        public string SellerName { get; set; }
        public string SellerMerchantId { get; }
        public double TotalCost => CheckoutProductViewModels.Sum(x => x.Price + x.Delivery);
        public string CurrencyCode { get; }

        public string UserId { get; }
    }

    public class CheckoutProductViewModel
    {
        public string Title { get; set; }
        public double Price { get; set; }
        public double Delivery { get; set; }
    }
}
