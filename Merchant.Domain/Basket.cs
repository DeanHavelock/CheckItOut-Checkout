using System.Collections.Generic;

namespace Merchant.Domain
{
    public class Basket
    {
        public string BasketId { get; set; }
        public string UserId { get; set; }
        public IEnumerable<BasketItem> BasketItems { get; set; }
    }
}
