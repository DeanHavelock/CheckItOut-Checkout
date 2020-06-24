using System;
using System.Collections.Generic;
using System.Text;

namespace CheckItOut.Payments.Domain
{
    public class Merchant
    {
        public string Id { get; set; }
        public string AccountNumber { get; set; }
        public string SortCode { get; set; }
        public string FullName { get; set; }
    }
}
