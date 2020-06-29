﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckItOut.Ui.Web.Models
{
    public class CreatePaymentViewModel
    {
        public string InvoiceId {get;set;}
        public string OrderId { get; set; }
        public string RecipientMerchantId { get; set; }
        public string Amount { get; set; }
        public string SenderFullName { get; set; }
        public string SenderCardNumber { get; set; }
        public string SenderCardCvv { get; set; }
        public string SenderCardExpiryMonth { get; set; }
        public string SenderCardExpiryYear { get; set; }
    }
}
